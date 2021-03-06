using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PaperStreet.Authentication.Application.Interfaces;
using PaperStreet.Authentication.Application.Queries;
using PaperStreet.Authentication.Domain.Models;
using PaperStreet.Domain.Core.Bus;
using PaperStreet.Domain.Core.Events.User.Logging;
using PaperStreet.Domain.Core.Models;

namespace PaperStreet.Authentication.Application.QueryHandlers
{
    public class RefreshTokenQueryHandler : IRequestHandler<RefreshTokenQuery, User>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly IEventBus _eventBus;

        public RefreshTokenQueryHandler(UserManager<AppUser> userManager, IJwtGenerator jwtGenerator, 
            IEventBus eventBus)
        {
            _userManager = userManager;
            _jwtGenerator = jwtGenerator;
            _eventBus = eventBus;
        }

        public async Task<User> Handle(RefreshTokenQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
                
            if (user == null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiry < DateTime.Now)
                throw new RestException(HttpStatusCode.Unauthorized);

            user.RefreshToken = _jwtGenerator.GenerateRefreshToken();
            user.RefreshTokenExpiry = DateTime.Now.AddDays(30);
            await _userManager.UpdateAsync(user);
            
            _eventBus.Publish(new UpdatedRefreshTokenEvent(user.Id));

            return new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = _jwtGenerator.CreateToken(user),
                RefreshToken = user.RefreshToken,
                Email= user.Email,
                Image = null
            };
        }
    }
}