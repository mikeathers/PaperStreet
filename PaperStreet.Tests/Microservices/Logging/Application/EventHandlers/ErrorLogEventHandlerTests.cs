using System.Threading.Tasks;
using NSubstitute;
using PaperStreet.Domain.Core.Events.Errors;
using PaperStreet.Logging.Application.EventHandlers;
using PaperStreet.Logging.Application.Interfaces;
using PaperStreet.Logging.Domain.Models;
using Xunit;

namespace PaperStreet.Tests.Microservices.Logging.Application.EventHandlers
{
    public class ErrorLogEventHandlerTests
    {
        [Fact]
        public async Task GivenErrorLogEventHandler_WhenHandleCalledWithCorrectData_ShouldCallErrorLogRepository()
        {
            const string userId = "101001";
            const string message = "test error message";
            var mockErrorLogRepository = Substitute.For<IErrorLogRepository>();
            var errorLogEvent = new ErrorLogEvent(userId, message);
            var errorLogEventHandler = new ErrorLogEventHandler(mockErrorLogRepository);
            await errorLogEventHandler.Handle(errorLogEvent);

            await mockErrorLogRepository.Received().SaveErrorLog(Arg.Any<ErrorLog>());
        }
    }
}