using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PaperStreet.Authentication.Application.Commands;
using PaperStreet.Authentication.Application.Interfaces;
using PaperStreet.Authentication.Data.Context;
using PaperStreet.Authentication.Domain.Models;
using PaperStreet.Domain.Core.Bus;
using PaperStreet.Domain.Core.Events.User.Logging;
using PaperStreet.Domain.Core.Models;

namespace PaperStreet.Authentication.Application.CommandHandlers
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUser.Command, User>
    {
        private readonly AuthenticationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly IEventBus _eventBus;
        private readonly IUserConfirmationEmail _userConfirmationEmail;

        public RegisterUserCommandHandler(AuthenticationDbContext context, UserManager<AppUser> userManager,
            IJwtGenerator jwtGenerator, IEventBus eventBus, IUserConfirmationEmail userConfirmationEmail)
        {
            _context = context;
            _userManager = userManager;
            _jwtGenerator = jwtGenerator;
            _eventBus = eventBus;
            _userConfirmationEmail = userConfirmationEmail;
        }

        public async Task<User> Handle(RegisterUser.Command request, CancellationToken cancellationToken)
        {
            if (await _context.Users.AnyAsync(x => x.Email == request.Email, cancellationToken: cancellationToken))
                throw new RestException(HttpStatusCode.BadRequest, new {Email = "Email already exists"});

            var user = new AppUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.Email,
                RefreshToken = _jwtGenerator.GenerateRefreshToken(),
                RefreshTokenExpiry = DateTime.Now.AddDays(30)
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded) throw new Exception("Problem registering user");
            
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