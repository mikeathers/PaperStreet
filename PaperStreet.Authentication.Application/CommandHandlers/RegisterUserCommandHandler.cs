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
using PaperStreet.Domain.Core.Events.User.Logging;
using PaperStreet.Domain.Core.Models;

namespace PaperStreet.Authentication.Application.CommandHandlers
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, User>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly IEventBus _eventBus;
        private readonly IUserConfirmationEmail _userConfirmationEmail;
        private readonly IFailedIdentityResult _failedIdentityResult;

        public RegisterUserCommandHandler(UserManager<AppUser> userManager, IJwtGenerator jwtGenerator, 
            IEventBus eventBus, IUserConfirmationEmail userConfirmationEmail, 
            IFailedIdentityResult failedIdentityResult)
        {
            _userManager = userManager;
            _jwtGenerator = jwtGenerator;
            _eventBus = eventBus;
            _userConfirmationEmail = userConfirmationEmail;
            _failedIdentityResult = failedIdentityResult;
        }

        public async Task<User> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            
            if (existingUser != null)
                throw new RestException(HttpStatusCode.BadRequest, new {Email = "UserId already exists"});

            var user = new AppUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.Email,
                RefreshToken = _jwtGenerator.GenerateRefreshToken(),
                RefreshTokenExpiry = DateTime.Now.AddDays(30)
            };

            var userRegistered = await _userManager.CreateAsync(user, request.Password);

            if (!userRegistered.Succeeded)
            {
                const string exceptionMessage = "Problem registering user";
                _failedIdentityResult.Handle(user, userRegistered.Errors, exceptionMessage);
            }
            
            _eventBus.Publish(new UserRegisteredEvent(user.Id));

            await _userConfirmationEmail.Send(user);

            return new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = _jwtGenerator.CreateToken(user),
                RefreshToken = user.RefreshToken,
                Email = user.Email,
                Image = null,
                EmailConfirmed = false
            };
        }
    }
}