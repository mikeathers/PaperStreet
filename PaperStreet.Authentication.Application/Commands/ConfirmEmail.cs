using FluentValidation;
using MediatR;
using PaperStreet.Authentication.Domain.Models;

namespace PaperStreet.Authentication.Application.Commands
{
    public class ConfirmEmail
    {
        public class Command : IRequest<User>
        {
            public string Email { get; set; }
            public string EmailConfirmationCode { get; set; }
        }

        public class QueryValidator : AbstractValidator<Command>
        {
            public QueryValidator()
            {
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.EmailConfirmationCode).NotEmpty();
            }
        }
    }
}