using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PaperStreet.Authentication.Application.Interfaces;
using PaperStreet.Authentication.Domain.Models;
using PaperStreet.Domain.Core.Bus;
using PaperStreet.Domain.Core.Events.Errors;
using PaperStreet.Domain.Core.Events.User.Logging;
using PaperStreet.Domain.Core.KeyValuePairs;
using PaperStreet.Domain.Core.Models;

namespace PaperStreet.Authentication.Application.QueryHandlers
{
    public class LoginUserQueryHandler : IRequestHandler<Queries.LoginUser.Query, User>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly IEventBus _eventBus;
        private readonly IFailedIdentityResult _failedIdentityResult;
        
        public LoginUserQueryHandler(UserManager<AppUser> userManager, IJwtGenerator jwtGenerator, IEventBus eventBus)
        {
            _userManager = userManager;
            _jwtGenerator = jwtGenerator;
            _eventBus = eventBus;
        }

        public async Task<User> Handle(Queries.LoginUser.Query request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
                throw new RestException(HttpStatusCode.Unauthorized);

            var userSignedIn = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!userSignedIn)
            {
                var errorMessage = ErrorMessages.UserLoginFailed;
                _eventBus.Publish(new LogErrorEvent(user.Id, errorMessage));
                throw new RestException(HttpStatusCode.Unauthorized);
            }
            
            user.RefreshToken = _jwtGenerator.GenerateRefreshToken();
            user.RefreshTokenExpiry = DateTime.Now.AddDays(30);
            
            await _userManager.UpdateAsync(user);
            
            _eventBus.Publish(new UserLoginEvent(user.Id));

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