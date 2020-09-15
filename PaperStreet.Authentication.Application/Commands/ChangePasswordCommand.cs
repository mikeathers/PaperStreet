using FluentValidation;
using MediatR;
using PaperStreet.Authentication.Application.Validators;

namespace PaperStreet.Authentication.Application.Commands
{
    public class ChangePasswordCommand : IRequest<bool>
    {
        
            public string Email { get; set; }
            public string CurrentPassword { get; set; }
            public string NewPassword { get; set; }
    }

    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.CurrentPassword).Password();
            RuleFor(x => x.NewPassword).Password();
        }
    }
    
}