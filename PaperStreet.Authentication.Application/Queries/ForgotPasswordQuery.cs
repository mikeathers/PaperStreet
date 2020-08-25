using FluentValidation;
using MediatR;

namespace PaperStreet.Authentication.Application.Queries
{
    public class ForgotPasswordQuery : IRequest<bool>
    {
        public string Email { get; set; }
    }
    
    public class ForgotPasswordQueryValidator : AbstractValidator<ForgotPasswordQuery>
    {
        public ForgotPasswordQueryValidator()
        {
            RuleFor(x => x.Email).NotEmpty();
        }
    }
}