using System.Collections;
using System.Collections.Generic;
using PaperStreet.Logging.Data.Context;
using PaperStreet.Logging.Domain.Models;

namespace PaperStreet.Tests.Microservices.Logging.SeedData
{
    public static class AuthenticationLogData
    {
        public static void SeedAuthenticationLogs(this LoggingDbContext context)
        {
            context.AuthenticationLogs.AddRange(AuthenticationLogs());
            context.SaveChanges();
        }
        
        public static void SeedSingleAuthenticationLog(this LoggingDbContext context)
        {
            context.AuthenticationLogs.AddRange(SingleAuthenticationLog());
            context.SaveChanges();
        }

        private static IEnumerable<AuthenticationLog> AuthenticationLogs()
        {
            return new List<AuthenticationLog>
            {
                new AuthenticationLog
                {
                    UserId = "12122-d3e3"
                }
            };
        }
        
        private static IEnumerable<AuthenticationLog> SingleAuthenticationLog()
        {
            return new List<AuthenticationLog>
            {
                new AuthenticationLog
                {
                    UserId = "12122-d3e3"
                }
            };
        }
    }
}