using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using PaperStreet.Logging.Application.Interfaces;
using PaperStreet.Logging.Application.QueryHandlers.User;
using Xunit;

namespace PaperStreet.Tests.Microservices.Logging.Application.QueryHandlers.User
{
    public class AllAuthenticationLogsQueryHandlerTests
    {
        [Fact]
        public async Task GivenAllAuthenticationLogsQueryHandler_WhenRequestIsMade_ThenShouldReturnCorrectData()
        {
            var mockLoggingRepository = Substitute.For<ILoggingRepository>();                
            var query = new PaperStreet.Logging.Application.Queries.User.AllAuthenticationLogs.Query();
            var allAuthenticationLogQueryHandler = new AllAuthenticationLogs(mockLoggingRepository);

            await allAuthenticationLogQueryHandler.Handle(query, CancellationToken.None);

            await mockLoggingRepository.Received().GetAllAuthenticationLogs();
        }
    }
}