using System.Threading.Tasks;
using NSubstitute;
using PaperStreet.Domain.Core.Events.User;
using PaperStreet.Logging.Application.EventHandlers.User;
using PaperStreet.Logging.Application.Interfaces;
using PaperStreet.Logging.Domain.Models;
using Xunit;

namespace PaperStreet.Tests.Microservices.Logging.Application.EventHandlers.User
{
    public class UserRegisteredEventHandlerTests
    {
        [Fact] public async Task GivenUserRegisteredEventHandler_WhenCorrectLogDataIsReceived_ThenShouldCallLoggingRepository()
        {
            const string email = "testuser@gmail.com";
            const string userId = "12340-223d234d";

            var mockLoggingRepository = Substitute.For<ILoggingRepository>();
            var userRegisteredEvent = new UserRegisteredEvent(email, userId);
            var userRegisteredEventHandler = new UserRegisteredEventHandler(mockLoggingRepository);
            
            await userRegisteredEventHandler.Handle(userRegisteredEvent);

            await mockLoggingRepository.Received().SaveAuthenticationLog(Arg.Any<AuthenticationLog>());
        }
    }
}