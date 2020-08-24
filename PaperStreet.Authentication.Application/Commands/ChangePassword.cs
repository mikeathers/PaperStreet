using FluentValidation;
using MediatR;
using PaperStreet.Authentication.Domain.Models;
using PaperStreet.Authentication.Domain.Validators;

namespace PaperStreet.Authentication.Application.Commands
{
    public class ChangePassword
    {
        public class Command : IRequest<bool>
        {
            public string Email { get; set; }
            public string CurrentPassword { get; set; }
            public string NewPassword { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.CurrentPassword).Password();
                RuleFor(x => x.NewPassword).Password();
            }
        }
    }
}