using FluentValidation;
using MediatR;
using PaperStreet.Authentication.Domain.Models;

namespace PaperStreet.Authentication.Application.Commands
{
    public class ConfirmEmailCommand : IRequest<User>
    {
        public string UserId { get; set; }
        public string EmailConfirmationCode { get; set; }
    }
    
    public class ConfirmEmailCommandValidator : AbstractValidator<ConfirmEmailCommand>
    {
        public ConfirmEmailCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.EmailConfirmationCode).NotEmpty();
        }
    }
}