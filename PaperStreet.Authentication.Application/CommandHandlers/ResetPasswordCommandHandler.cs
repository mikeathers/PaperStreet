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
using PaperStreet.Domain.Core.Events.User.Communication;
using PaperStreet.Domain.Core.Events.User.Logging;
using PaperStreet.Domain.Core.Models;

namespace PaperStreet.Authentication.Application.CommandHandlers
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPassword.Command, bool>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEventBus _eventBus;
        private readonly IEmailBuilder _emailBuilder;
        
        public ResetPasswordCommandHandler(UserManager<AppUser> userManager, IEventBus eventBus, IEmailBuilder emailBuilder)
        {
            _userManager = userManager;
            _eventBus = eventBus;
            _emailBuilder = emailBuilder;
        }

        public async Task<bool> Handle(ResetPassword.Command request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null) throw new RestException(HttpStatusCode.Unauthorized);

            var passwordReset =
                await _userManager.ResetPasswordAsync(user, request.ResetPasswordToken, request.NewPassword);

            if (!passwordReset.Succeeded) throw new Exception("Problem resetting password");

            var passwordChangedEmail = _emailBuilder.PasswordChangedEmail(user.FirstName);
            
            var emailToSend = new Email
            {
                FirstName = user.FirstName,
                HtmlContent = passwordChangedEmail,
                PlainTextContent = null,
                Subject = EmailSubjects.PasswordHasBeenChanged,
                To = user.Email,
                UserId = user.Id
            };
            
            _eventBus.Publish(new PasswordChangedEvent(user.Id));
            _eventBus.Publish(new SendEmailEvent(emailToSend));
            return await Task.FromResult(true);
        }
    }
}