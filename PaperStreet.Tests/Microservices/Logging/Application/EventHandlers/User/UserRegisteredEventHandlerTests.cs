using System.Linq;
using System.Threading.Tasks;
using PaperStreet.Domain.Core.Events.User;
using PaperStreet.Logging.Application.EventHandlers.User;
using PaperStreet.Logging.Data.Context;
using PaperStreet.Tests.Microservices.Logging.SeedData;
using TestSupport.EfHelpers;
using Xunit;

namespace PaperStreet.Tests.Microservices.Logging.Application.EventHandlers.User
{
    public class UserRegisteredEventHandlerTests
    {
        [Fact]
        public async Task UserRegisteredEventHandler_WhenCorrectLogDataIsReceived_ThenShouldPersistLog()
        {
            var options = SqliteInMemory.CreateOptions<LoggingDbContext>();
            await using var context = new LoggingDbContext(options);
            {
                await context.Database.EnsureCreatedAsync();
                context.SeedAuthenticationLogs();
                
                const string email = "testuser@gmail.com";
                const string userId = "12340-223d234d";

                var authenticationLogCount = context.AuthenticationLogs.Count();
                var userRegisteredEvent = new UserRegisteredEvent(email, userId);
                var userRegisteredEventHandler = new UserRegisteredEventHandler(context);
                
                await userRegisteredEventHandler.Handle(userRegisteredEvent);

                var updatedAuthenticationLogCount = context.AuthenticationLogs.Count();
                
                Assert.Equal(authenticationLogCount + 1, updatedAuthenticationLogCount);
            }
            
        }
        
    }
}