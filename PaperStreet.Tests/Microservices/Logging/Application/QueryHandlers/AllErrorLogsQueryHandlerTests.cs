using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using PaperStreet.Logging.Application.Interfaces;
using PaperStreet.Logging.Application.Queries;
using PaperStreet.Logging.Application.QueryHandlers;
using Xunit;

namespace PaperStreet.Tests.Microservices.Logging.Application.QueryHandlers
{
    public class AllErrorLogsQueryHandlerTests
    {
        [Fact]
        public async Task GivenAllErrorLogsQueryHandler_WhenReceivesCorrectQuery_ThenShouldCallErrorLogRepository()
        {
            var mockLoggingRepository = Substitute.For<IErrorLogRepository>();                
            var query = new AllErrorLogsQuery();
            var allErrorLogsQueryHandler = new AllErrorLogsQueryHandler(mockLoggingRepository);

            await allErrorLogsQueryHandler.Handle(query, CancellationToken.None);

            await mockLoggingRepository.Received().GetAllErrorLogs();
        }
    }
}