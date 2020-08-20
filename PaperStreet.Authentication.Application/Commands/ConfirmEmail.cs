using FluentValidation;
using MediatR;
using PaperStreet.Authentication.Domain.Models;

namespace PaperStreet.Authentication.Application.Commands
{
    public class ConfirmEmail
    {
        public class Command : IRequest<User>
        {
            public string UserId { get; set; }
            public string EmailConfirmationCode { get; set; }
        }

        public class QueryValidator : AbstractValidator<Command>
        {
            public QueryValidator()
            {
                RuleFor(x => x.UserId).NotEmpty();
                RuleFor(x => x.EmailConfirmationCode).NotEmpty();
            }
        }
    }
}