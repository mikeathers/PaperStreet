using PaperStreet.Authentication.Domain.Models;

namespace PaperStreet.Authentication.Application.Interfaces
{
    public interface IJwtGenerator
    {
        string CreateToken(AppUser user);
        string GenerateRefreshToken();
    }
}