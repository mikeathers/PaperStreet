using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PaperStreet.Authentication.Domain.Models;

namespace PaperStreet.Authentication.Data.Context
{
    public class AuthenticationDbContext : IdentityDbContext<AppUser>
    {
        public AuthenticationDbContext(DbContextOptions options) : base(options)
        {
        }

        public AuthenticationDbContext()
        {
            
        }
    }
}