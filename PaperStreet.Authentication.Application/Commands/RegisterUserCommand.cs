using FluentValidation;
using MediatR;
using PaperStreet.Authentication.Domain.Models;
using PaperStreet.Authentication.Domain.Validators;

namespace PaperStreet.Authentication.Application.Commands
{
    public class RegisterUserCommand : IRequest<User>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
    
    public class RegisterCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).Password();
        }
    }
}