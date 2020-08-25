using MediatR;
using NSubstitute;
using PaperStreet.Logging.Api.Controllers;
using PaperStreet.Logging.Application.Queries;
using Xunit;

namespace PaperStreet.Tests.Microservices.Logging.Api.Controllers
{
    public class LoggingControllerTests
    {
        [Fact]
        public void GivenAllAuthenticationLogsGetMethod_WhenReceivesCorrectQuery_ThenFireMediatorSendMethod()
        {
            var mockMediator = Substitute.For<IMediator>();
            var loggingController = new LoggingController(mockMediator);

            loggingController.GetAllAuthenticationLogs();

            mockMediator.Received().Send(Arg.Any<AllAuthenticationLogs.Query>());
        }
        
        [Fact]
        public void GivenAllErrorLogsGetMethod_WhenReceivesCorrectQuery_ThenFireMediatorSendMethod()
        {
            var mockMediator = Substitute.For<IMediator>();
            var loggingController = new LoggingController(mockMediator);

            loggingController.GetAllErrorLogs();

            mockMediator.Received().Send(Arg.Any<AllErrorLogs.Query>());
        }
    }
}