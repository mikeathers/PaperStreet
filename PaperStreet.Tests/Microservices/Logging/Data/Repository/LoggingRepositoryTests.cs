using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PaperStreet.Logging.Data.Context;
using PaperStreet.Logging.Data.Repository;
using PaperStreet.Logging.Domain.Models;
using PaperStreet.Tests.Microservices.Logging.SeedData;
using TestSupport.EfHelpers;
using Xunit;

namespace PaperStreet.Tests.Microservices.Logging.Data.Repository
{
    public class LoggingRepositoryTests
    {
        [Fact]
        public async Task GivenLoggingRepository_WhenCorrectAuthenticationLogDataIsReceived_ThenShouldPersistLog()
        {
            var options = SqliteInMemory.CreateOptions<LoggingDbContext>();
            await using var context = new LoggingDbContext(options);
            {
                await context.Database.EnsureCreatedAsync();
                context.SeedAuthenticationLogs();

                var authenticationLog = new AuthenticationLog
                {
                    UserId = "001",
                    MessageType = "UserRegisteredEvent",
                    Timestamp = DateTime.Now,
                };

                var authenticationLogCount = context.AuthenticationLogs.Count();
                var loggingRepository = new LoggingRepository(context);
                await loggingRepository.SaveAuthenticationLog(authenticationLog);
                var updatedAuthenticationLogCount = context.AuthenticationLogs.Count();
                
                Assert.Equal(authenticationLogCount + 1, updatedAuthenticationLogCount);
            }
        }
        
        [Fact]
        public async Task GivenAllAuthenticationLogsQueryHandler_WhenRequestIsMade_ThenShouldReturnCorrectData()
        {
            var options = SqliteInMemory.CreateOptions<LoggingDbContext>();
            await using var context = new LoggingDbContext(options);
            {
                await context.Database.EnsureCreatedAsync();
                context.SeedSingleAuthenticationLog();
                
                var loggingRepository = new LoggingRepository(context);
                var logs = await loggingRepository.GetAllAuthenticationLogs();

                Assert.IsType<List<AuthenticationLog>>(logs);
                Assert.Single(logs);
            }
        }
    }
}