using System.Security.Claims;

namespace PaperStreet.Authentication.Application.Interfaces
{
    public interface ITokenHandler
    {
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}