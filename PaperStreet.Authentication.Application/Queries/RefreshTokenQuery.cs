using FluentValidation;
using MediatR;
using PaperStreet.Authentication.Domain.Models;

namespace PaperStreet.Authentication.Application.Queries
{
    public class RefreshTokenQuery : IRequest<User>
    {
        public string Email { get; set; }
        public string Token { get; set; }    
        public string RefreshToken { get; set; }
    }

    public class RefreshTokenQueryValidator : AbstractValidator<RefreshTokenQuery>
    {
        public RefreshTokenQueryValidator()
        {
            RuleFor(x => x.Token).NotEmpty();
            RuleFor(x => x.RefreshToken).NotEmpty();
        }
    }
}