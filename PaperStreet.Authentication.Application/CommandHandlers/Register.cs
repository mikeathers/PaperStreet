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

namespace PaperStreet.Authentication.Application.CommandHandlers
{
    public class Register : IRequestHandler<Commands.Register.Command, User>
    {
        private readonly AuthenticationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtGenerator _jwtGenerator;

        public Register(AuthenticationDbContext context, UserManager<AppUser> userManager, IJwtGenerator jwtGenerator)
        {
            _context = context;
            _userManager = userManager;
            _jwtGenerator = jwtGenerator;
        }

        public async Task<User> Handle(Commands.Register.Command request, CancellationToken cancellationToken)
        {
            if (await _context.Users.Where(x => x.Email == request.Email).AnyAsync(cancellationToken: cancellationToken))
                throw new RestException(HttpStatusCode.BadRequest, new {Email = "Email already exists"});
            
            if (await _context.Users.Where(x => x.UserName == request.Username).AnyAsync(cancellationToken: cancellationToken))
                throw new RestException(HttpStatusCode.BadRequest, new {Username = "Username already exists"});

            var user = new AppUser
            {
                DisplayName = request.DisplayName,
                Email = request.Email,
                UserName = request.Username,
                RefreshToken = _jwtGenerator.GenerateRefreshToken(),
                RefreshTokenExpiry = DateTime.Now.AddDays(30)
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                return new User
                {
                    DisplayName = user.DisplayName,
                    Token = _jwtGenerator.CreateToken(user),
                    RefreshToken = user.RefreshToken,
                    Username = user.UserName,
                    Image = null
                };
            }

            throw new Exception("Problem creating user");
        }
    }
}