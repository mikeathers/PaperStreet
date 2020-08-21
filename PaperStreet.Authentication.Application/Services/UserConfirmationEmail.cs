using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PaperStreet.Authentication.Application.Interfaces;
using PaperStreet.Authentication.Domain.Models;
using PaperStreet.Domain.Core.Bus;
using PaperStreet.Domain.Core.Events.User;
using PaperStreet.Domain.Core.Models;

namespace PaperStreet.Authentication.Application.Services
{
    public class UserConfirmationEmail : IUserConfirmationEmail
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailBuilder _emailBuilder;
        private readonly IEventBus _eventBus;
        
        public UserConfirmationEmail(UserManager<AppUser> userManager, IEmailBuilder emailBuilder, IEventBus eventBus)
        {
            _userManager = userManager;
            _emailBuilder = emailBuilder;
            _eventBus = eventBus;
        }

        public async Task Send(AppUser user)
        {
            var confirmEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationEmailHtml = _emailBuilder.ConfirmationEmail(user.FirstName, user.Id, confirmEmailToken);

            var emailToSend = new Email
            {
                To = user.Email,
                FirstName = user.FirstName,
                Subject = "Confirm your email",
                HtmlContent = confirmationEmailHtml,
                PlainTextContent = null,
                UserId = user.Id
            };
            
            _eventBus.Publish(new SendConfirmationEmailEvent(user.Id, user.Email, emailToSend));
        }
    }
}