using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PaperStreet.Authentication.Application.Commands;
using PaperStreet.Authentication.Application.Interfaces;
using PaperStreet.Authentication.Domain.KeyValuePairs;
using PaperStreet.Authentication.Domain.Models;
using PaperStreet.Domain.Core.Bus;
using PaperStreet.Domain.Core.Events.Errors;
using PaperStreet.Domain.Core.Events.User.Communication;
using PaperStreet.Domain.Core.Events.User.Logging;
using PaperStreet.Domain.Core.Formatters;
using PaperStreet.Domain.Core.Models;

namespace PaperStreet.Authentication.Application.CommandHandlers
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePassword.Command, bool>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailBuilder _emailBuilder;
        private readonly IEventBus _eventBus;
        private readonly IFailedIdentityResult _failedIdentityResult;

        public ChangePasswordCommandHandler(UserManager<AppUser> userManager, IEmailBuilder emailBuilder,
            IEventBus eventBus, IFailedIdentityResult failedIdentityResult)
        {
            _userManager = userManager;
            _emailBuilder = emailBuilder;
            _eventBus = eventBus;
            _failedIdentityResult = failedIdentityResult;
        }
        
        public async Task<bool> Handle(ChangePassword.Command request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            
            if (user == null) throw new RestException(HttpStatusCode.Unauthorized);
            
            var passwordChanged =
                await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

            if (!passwordChanged.Succeeded)
            {
                const string exceptionMessage = "Problem changing password";
                _failedIdentityResult.Handle(user, passwordChanged.Errors, exceptionMessage);
            }
                
            var passwordChangedHtml = _emailBuilder.PasswordChangedEmail(user.FirstName);
            
            var emailToSend = new Email
            {
                FirstName = user.FirstName,
                HtmlContent = passwordChangedHtml,
                PlainTextContent = null,
                Subject = EmailSubjects.PasswordHasBeenChanged,
                To = user.Email,
                UserId = user.Id
            };
                    
            _eventBus.Publish(new SendEmailEvent(emailToSend));
            _eventBus.Publish(new PasswordChangedEvent(user.Id));

            return await Task.FromResult(true);
        }
    }
}