using FluentValidation;
using MediatR;
using PaperStreet.Authentication.Domain.Models;

namespace PaperStreet.Authentication.Application.Queries
{
    public class LoginUserQuery : IRequest<User>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    
    public class LoginUserQueryValidator : AbstractValidator<LoginUserQuery>
    {
        public LoginUserQueryValidator()
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}