using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using PaperStreet.Logging.Application.Interfaces;
using PaperStreet.Logging.Application.Queries;
using PaperStreet.Logging.Application.QueryHandlers;
using Xunit;

namespace PaperStreet.Tests.Microservices.Logging.Application.QueryHandlers
{
    public class AllAuthenticationLogsQueryHandlerTests
    {
        [Fact]
        public async Task
            GivenAllAuthenticationLogsQueryHandler_WhenReceivesCorrectQuery_ThenShouldCallAuthenticationLogRepository()
        {
            var mockLoggingRepository = Substitute.For<IAuthenticationLogRepository>();                
            var query = new AllAuthenticationLogsQuery();
            var allAuthenticationLogQueryHandler = new AllAuthenticationLogs(mockLoggingRepository);

            await allAuthenticationLogQueryHandler.Handle(query, CancellationToken.None);

            await mockLoggingRepository.Received().GetAllAuthenticationLogs();
        }
    }
}