using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PaperStreet.Authentication.Application.Commands;
using PaperStreet.Authentication.Application.Interfaces;
using PaperStreet.Authentication.Application.Queries;
using PaperStreet.Authentication.Domain.KeyValuePairs;
using PaperStreet.Authentication.Domain.Models;
using PaperStreet.Domain.Core.Bus;
using PaperStreet.Domain.Core.Events.User.Communication;
using PaperStreet.Domain.Core.Events.User.Logging;
using PaperStreet.Domain.Core.Models;

namespace PaperStreet.Authentication.Application.QueryHandlers
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPassword.Query, bool>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailBuilder _emailBuilder;
        private readonly IEventBus _eventBus;

        public ForgotPasswordCommandHandler(UserManager<AppUser> userManager, IEmailBuilder emailBuilder, IEventBus eventBus)
        {
            _userManager = userManager;
            _emailBuilder = emailBuilder;
            _eventBus = eventBus;
        }

        public async Task<bool> Handle(ForgotPassword.Query request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            
            if (user == null) throw new RestException(HttpStatusCode.Unauthorized);

            var resetPasswordConfirmationCode = await _userManager.GeneratePasswordResetTokenAsync(user);

            var resetPasswordEmail = _emailBuilder.ResetPasswordEmail(user.FirstName, user.Id, resetPasswordConfirmationCode);

            var emailToSend = new Email
            {
                FirstName = user.FirstName,
                HtmlContent = resetPasswordEmail,
                PlainTextContent = null,
                Subject = EmailSubjects.ResetPassword,
                To = user.Email,
                UserId = user.Id
            };
            
            _eventBus.Publish(new SendEmailEvent(emailToSend));
            _eventBus.Publish(new ResetPasswordRequestEvent(user.Id));

            return await Task.FromResult(true);
        }
    }
}