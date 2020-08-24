using FluentValidation;
using MediatR;
using PaperStreet.Authentication.Domain.Validators;

namespace PaperStreet.Authentication.Application.Commands
{
    public class ResetPassword
    {
        public class Command : IRequest<bool>
        {
            public string Email { get; set; }
            public string NewPassword { get; set; }
            public string ResetPasswordToken { get; set; }    
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.NewPassword).Password();
                RuleFor(x => x.ResetPasswordToken).NotEmpty();
            }
        }
    }
}