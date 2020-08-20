using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PaperStreet.Authentication.Application.Interfaces;
using PaperStreet.Authentication.Data.Context;
using PaperStreet.Authentication.Domain.Models;
using PaperStreet.Domain.Core.Bus;
using PaperStreet.Domain.Core.Events.User;
using PaperStreet.Domain.Core.Models;

namespace PaperStreet.Authentication.Application.CommandHandlers
{
    public class RegisterUserCommandHandler : IRequestHandler<Commands.RegisterUser.Command, User>
    {
        private readonly AuthenticationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly IEventBus _eventBus;

        public RegisterUserCommandHandler(AuthenticationDbContext context, UserManager<AppUser> userManager,
            IJwtGenerator jwtGenerator, IEventBus eventBus)
        {
            _context = context;
            _userManager = userManager;
            _jwtGenerator = jwtGenerator;
            _eventBus = eventBus;
        }

        public async Task<User> Handle(Commands.RegisterUser.Command request, CancellationToken cancellationToken)
        {
            if (await _context.Users.Where(x => x.Email == request.Email).AnyAsync(cancellationToken: cancellationToken))
                throw new RestException(HttpStatusCode.BadRequest, new {Email = "Email already exists"});

            var user = new AppUser
            {
                DisplayName = request.DisplayName,
                Email = request.Email,
                UserName = request.Email,
                RefreshToken = _jwtGenerator.GenerateRefreshToken(),
                RefreshTokenExpiry = DateTime.Now.AddDays(30)
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded) throw new Exception("Problem creating user");
            
            _eventBus.Publish(new UserRegisteredEvent(user.Id, user.Email));
                
            return new User
            {
                DisplayName = user.DisplayName,
                Token = _jwtGenerator.CreateToken(user),
                RefreshToken = user.RefreshToken,
                Email = user.Email,
                Image = null,
                EmailConfirmed = false
            };

        }
    }
}