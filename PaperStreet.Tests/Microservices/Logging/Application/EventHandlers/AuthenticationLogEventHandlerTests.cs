using System.Threading.Tasks;
using NSubstitute;
using PaperStreet.Domain.Core.Events.User.Logging;
using PaperStreet.Logging.Application.EventHandlers;
using PaperStreet.Logging.Application.Interfaces;
using PaperStreet.Logging.Domain.Models;
using Xunit;

namespace PaperStreet.Tests.Microservices.Logging.Application.EventHandlers
{
    public class UserRegisteredEventHandlerTests
    {
        [Fact] 
        public async Task GivenUserRegisteredEventHandler_WhenCorrectLogDataIsReceived_ThenShouldCallLoggingRepository()
        {
            const string userId = "12340-223d234d";

            var mockAuthenticationLogRepository = Substitute.For<IAuthenticationLogRepository>();
            var authenticationLogEvent = new UserRegisteredEvent(userId);
            var userRegisteredEventHandler = new AuthenticationLogEventHandler(mockAuthenticationLogRepository);
            
            await userRegisteredEventHandler.Handle(authenticationLogEvent);

            await mockAuthenticationLogRepository.Received().SaveAuthenticationLog(Arg.Any<AuthenticationLog>());
        }
    }
}