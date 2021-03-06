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
        
        public static void SeedSingleUserData(this AuthenticationDbContext context)
        {
            context.Users.AddRange(SingleUser());
            context.SaveChangesAsync();
        }

        private static IEnumerable<AppUser> Users()
        {
            return new List<AppUser>
            {
                new AppUser()
                {
                    FirstName = "Test",
                    LastName = "User",
                    UserName = "test@gmail.com",
                    Email = "test@gmail.com",
                }
            };

        }
        
        private static IEnumerable<AppUser> SingleUser()
        {
            return new List<AppUser>
            {
                new AppUser()
                {
                    FirstName = "Test",
                    LastName = "User",
                    UserName = "test@gmail.com",
                    Email = "test@gmail.com",
                }
            };

        }
    }
}