using MediatR;
using NSubstitute;
using PaperStreet.Authentication.Api.Controllers;
using PaperStreet.Authentication.Application.Commands;
using PaperStreet.Authentication.Application.Queries;
using Xunit;

namespace PaperStreet.Tests.Microservices.Authentication.Api.Controllers
{
    public class AuthenticationControllerTests
    {
        [Fact]
        public void GivenRegisterPostMethod_WhenReceivesCorrectCommand_ThenMediatorSendMethodShouldFire()
        {
            var registerCommand = new RegisterUser.Command
            {
                DisplayName = "Test User",
                Email = "test@gmail.com",
                Password = "password123"
            };
            
            var mockMediator = Substitute.For<IMediator>();
            var authenticationController = new AuthenticationController(mockMediator);

            authenticationController.Register(registerCommand);

            mockMediator.Received().Send(registerCommand);
        }
        
        [Fact]
        public void GivenLoginPostMethod_WhenReceivesCorrectQuery_ThenMediatorSendMethodShouldFire()
        {
            var loginQuery = new LoginUser.Query
            {
                Email = "test@gmail.com",
                Password = "password123"
            };
            
            var mockMediator = Substitute.For<IMediator>();
            var authenticationController = new AuthenticationController(mockMediator);

            authenticationController.Login(loginQuery);

            mockMediator.Received().Send(loginQuery);
        }
        
        [Fact]
        public void GivenConfirmEmailPostMethod_WhenReceivesCorrectCommand_ThenMediatorSendMethodShouldFire()
        {
            const string userId = "001";
            const string emailConfirmationCode = "101010";

            var mockMediator = Substitute.For<IMediator>();
            var authenticationController = new AuthenticationController(mockMediator);

            authenticationController.ConfirmEmail(userId, emailConfirmationCode);

            mockMediator.Received().Send(Arg.Any<ConfirmEmail.Command>());
        }
    }
}