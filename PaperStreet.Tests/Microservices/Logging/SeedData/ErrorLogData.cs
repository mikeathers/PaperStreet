using System;
using System.Collections.Generic;
using PaperStreet.Logging.Data.Context;
using PaperStreet.Logging.Domain.Models;

namespace PaperStreet.Tests.Microservices.Logging.SeedData
{
    public static class ErrorLogData
    {
        public static void SeedErrorLogs(this LoggingDbContext context)
        {
            context.ErrorLogs.AddRange(ErrorLogs());
            context.SaveChanges();
        }
        
        public static void SeedSingleErrorLog(this LoggingDbContext context)
        {
            context.ErrorLogs.AddRange(SingleErrorLog());
            context.SaveChanges();    
        }

        private static IEnumerable<ErrorLog> SingleErrorLog()
        {
            return new List<ErrorLog>
            {
                new ErrorLog
                {
                    ErrorMessage = "Error Message",
                    MessageType = "ErrorLog",
                    Timestamp = DateTime.Now,
                    UserId = "1010101"
                }
            };
        }
        
        private static IEnumerable<ErrorLog> ErrorLogs()
        {
            return new List<ErrorLog>
            {
                new ErrorLog
                {
                    ErrorMessage = "Error Message",
                    MessageType = "ErrorLog",
                    Timestamp = DateTime.Now,
                    UserId = "1010101"
                },
                new ErrorLog
                {
                    ErrorMessage = "Error Message 2",
                    MessageType = "ErrorLog",
                    Timestamp = DateTime.Now,
                    UserId = "1010101"
                }
            };
        }
    }
}