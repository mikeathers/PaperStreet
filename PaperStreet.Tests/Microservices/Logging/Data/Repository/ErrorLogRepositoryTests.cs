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
    public class ErrorLogRepositoryTests
    {
        [Fact]
        public async Task GivenSaveErrorLog_WhenCorrectDataReceived_ThenShouldPersistLog()
        {
            var options = SqliteInMemory.CreateOptions<LoggingDbContext>();
            await using var context = new LoggingDbContext(options);
            {
                await context.Database.EnsureCreatedAsync();
                context.SeedSingleErrorLog();

                var errorLog = new ErrorLog
                {
                    ErrorMessage = "Test Error Message",
                    MessageType = "ErrorLog",
                    Timestamp = DateTime.Now,
                    UserId = "1010101"
                };
                
                var currentErrorLogCount = context.ErrorLogs.Count();
                var errorLogRepository = new ErrorLogRepository(context);
                
                await errorLogRepository.SaveErrorLog(errorLog);
                var updatedErrorLogCount = context.ErrorLogs.Count();
                
                Assert.Equal(currentErrorLogCount + 1, updatedErrorLogCount);
            }
        }

        [Fact] public async Task GivenGetAllErrorLogs_WhenCalled_ThenShouldReturnAllErrorLogs()
        {
            var options = SqliteInMemory.CreateOptions<LoggingDbContext>();
            await using var context = new LoggingDbContext(options);
            {
                await context.Database.EnsureCreatedAsync();
                context.SeedErrorLogs();

                var errorLogCount = context.ErrorLogs.Count();
                var errorLogRepository = new ErrorLogRepository(context);
                
                var logs = await errorLogRepository.GetAllErrorLogs();

                Assert.NotNull(logs);
                Assert.Equal(errorLogCount, logs.Count);
                Assert.IsType<List<ErrorLog>>(logs);
            }
        }
    }
}