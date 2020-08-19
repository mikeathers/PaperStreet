using FluentValidation;
using MediatR;
using PaperStreet.Authentication.Domain.Models;
using PaperStreet.Authentication.Domain.Validators;

namespace PaperStreet.Authentication.Application.Commands
{
    public class RegisterUser
    {
        public class Command : IRequest<User>
        {
            public string DisplayName { get; set; }
            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class RegisterCommandValidator : AbstractValidator<Command>
        {
            public RegisterCommandValidator()
            {
                RuleFor(x => x.DisplayName).NotEmpty();
                RuleFor(x => x.Username).NotEmpty();
                RuleFor(x => x.Email).NotEmpty().EmailAddress();
                RuleFor(x => x.Password).Password();
            }
        }
    }
}