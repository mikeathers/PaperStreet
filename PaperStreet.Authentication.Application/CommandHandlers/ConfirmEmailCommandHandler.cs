using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PaperStreet.Authentication.Application.Commands;
using PaperStreet.Authentication.Application.Interfaces;
using PaperStreet.Authentication.Domain.Models;
using PaperStreet.Domain.Core.Bus;
using PaperStreet.Domain.Core.Events.User;
using PaperStreet.Domain.Core.Models;

namespace PaperStreet.Authentication.Application.CommandHandlers
{
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmail.Command, User>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly IEventBus _eventBus;
        
        public ConfirmEmailCommandHandler(UserManager<AppUser> userManager, IJwtGenerator jwtGenerator, IEventBus eventBus)
        {
            _userManager = userManager;
            _jwtGenerator = jwtGenerator;
            _eventBus = eventBus;
        }

        public async Task<User> Handle(ConfirmEmail.Command request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            
            if (user == null)
                throw new RestException(HttpStatusCode.Unauthorized);

            var userConfirmed = await _userManager.ConfirmEmailAsync(user, request.EmailConfirmationCode);

            if (!userConfirmed.Succeeded) throw new RestException(HttpStatusCode.Unauthorized);
            
            user.RefreshToken = _jwtGenerator.GenerateRefreshToken();
            user.RefreshTokenExpiry = DateTime.Now.AddDays(30);
            
            await _userManager.UpdateAsync(user);
            
            _eventBus.Publish(new EmailConfirmedEvent(user.Id, user.Email));

            return new User
            {
                FirstName = user.FirstName,
                Token = _jwtGenerator.CreateToken(user),
                RefreshToken = user.RefreshToken,
                Email = user.Email,
                Image = null,
                EmailConfirmed = user.EmailConfirmed
            };
        }
    }
}