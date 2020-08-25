using FluentValidation;
using MediatR;
using PaperStreet.Authentication.Domain.Models;

namespace PaperStreet.Authentication.Application.Commands
{
    public class ConfirmEmailCommand : IRequest<User>
    {
        public string Email { get; set; }
        public string EmailConfirmationCode { get; set; }
    }
    
    public class ConfirmEmailCommandValidator : AbstractValidator<ConfirmEmailCommand>
    {
        public ConfirmEmailCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.EmailConfirmationCode).NotEmpty();
        }
    }
}