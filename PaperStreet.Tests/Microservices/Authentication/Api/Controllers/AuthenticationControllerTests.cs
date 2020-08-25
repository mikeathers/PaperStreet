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
            var registerCommand = new RegisterUserCommand
            {
                FirstName = "Test User",
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
            var loginQuery = new LoginUserQuery
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

            mockMediator.Received().Send(Arg.Any<ConfirmEmailCommand>());
        }
        
        [Fact]
        public void GivenForgotPasswordPostMethod_WhenReceivesCorrectQuery_ThenMediatorSendMethodShouldFire()
        {
            var forgotPasswordQuery = new ForgotPasswordQuery
            {
                Email = "test@gmail.com",
            };
            
            var mockMediator = Substitute.For<IMediator>();
            var authenticationController = new AuthenticationController(mockMediator);

            authenticationController.ForgotPassword(forgotPasswordQuery);

            mockMediator.Received().Send(forgotPasswordQuery);
        }
        
        [Fact]
        public void GivenResetPasswordPostMethod_WhenReceivesCorrectCommand_ThenMediatorSendMethodShouldFire()
        {
            var resetPasswordQuery = new ResetPasswordCommand
            {
                Email = "test@gmail.com",
                NewPassword = "Password123!",
                ResetPasswordToken = "10101010"
            };
            
            var mockMediator = Substitute.For<IMediator>();
            var authenticationController = new AuthenticationController(mockMediator);

            authenticationController.ResetPassword(resetPasswordQuery);

            mockMediator.Received().Send(resetPasswordQuery);
        }
        
        [Fact]
        public void GivenChangePasswordPostMethod_WhenReceivesCorrectCommand_ThenMediatorSendMethodShouldFire()
        {
            var changePasswordCommand = new ChangePasswordCommand
            {
                Email = "test@gmail.com",
                NewPassword = "Password123!",
                CurrentPassword = "Password1!"
            };
            
            var mockMediator = Substitute.For<IMediator>();
            var authenticationController = new AuthenticationController(mockMediator);

            authenticationController.ChangePassword(changePasswordCommand);

            mockMediator.Received().Send(changePasswordCommand);
        }
    }
}