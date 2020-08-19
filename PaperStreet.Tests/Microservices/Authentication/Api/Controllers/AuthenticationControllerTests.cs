using MediatR;
using NSubstitute;
using PaperStreet.Authentication.Api.Controllers;
using PaperStreet.Authentication.Application.Commands;
using Xunit;

namespace PaperStreet.Tests.Microservices.Authentication.Api.Controllers
{
    public class AuthenticationControllerTests
    {
        [Fact]
        public void GivenRegisterPostMethod_WhenReceivesCorrectCommand_ThenFireMediatorSendMethod()
        {
            var registerCommand = new Register.Command
            {
                DisplayName = "Test User",
                Username = "test@gmail.com",
                Email = "test@gmail.com",
                Password = "password123"
            };
            
            var mockMediator = Substitute.For<IMediator>();
            var authenticationController = new AuthenticationController(mockMediator);

            authenticationController.Register(registerCommand);

            mockMediator.Received().Send(registerCommand);
        }
    }
}