using System.Collections.Generic;
using PaperStreet.Authentication.Data.Context;
using PaperStreet.Authentication.Domain.Models;

namespace PaperStreet.Tests.Microservices.Authentication.SeedData
{
    public static class UserData
    {
        public static void SeedUserData(this AuthenticationDbContext context)
        {
            context.Users.AddRange(Users());
            context.SaveChangesAsync();
        }

        private static IEnumerable<AppUser> Users()
        {
            return new List<AppUser>
            {
                new AppUser()
                {
                    DisplayName = "Test User",
                    UserName = "test@gmail.com",
                    Email = "test@gmail.com",
                }
            };

        }
    }
}