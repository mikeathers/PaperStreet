using System.Security.Claims;
using MediatR;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using PaperStreet.Authentication.Api.Controllers;
using PaperStreet.Authentication.Application.Commands;
using PaperStreet.Authentication.Application.Interfaces;
using PaperStreet.Authentication.Application.Queries;
using Xunit;

namespace PaperStreet.Tests.Microservices.Authentication.Api.Controllers
{
    public class AuthenticationControllerTests
    {
        private readonly IMediator _mockMediator;
        private readonly ITokenHandler _mockTokenHandler;
        
        public AuthenticationControllerTests()
        {
            _mockTokenHandler =  Substitute.For<ITokenHandler>();;
            _mockMediator = Substitute.For<IMediator>();
        }
        
        [Fact]
        public void GivenRegisterPostMethod_WhenReceivesCorrectCommand_ThenMediatorSendMethodShouldFire()
        {
            var registerCommand = new RegisterUserCommand
            {
                FirstName = "Test User",
                Email = "test@gmail.com",
                Password = "password123"
            };
            
            var authenticationController = new AuthenticationController(_mockMediator, _mockTokenHandler);

            authenticationController.Register(registerCommand);

            _mockMediator.Received().Send(registerCommand);
        }
        
        [Fact]
        public void GivenLoginPostMethod_WhenReceivesCorrectQuery_ThenMediatorSendMethodShouldFire()
        {
            var loginQuery = new LoginUserQuery
            {
                Email = "test@gmail.com",
                Password = "password123"
            };
            
            var authenticationController = new AuthenticationController(_mockMediator, _mockTokenHandler);

            authenticationController.Login(loginQuery);

            _mockMediator.Received().Send(loginQuery);
        }
        
        [Fact]
        public void GivenConfirmEmailPostMethod_WhenReceivesCorrectCommand_ThenMediatorSendMethodShouldFire()
        {
            const string userId = "001";
            const string emailConfirmationCode = "101010";

            var authenticationController = new AuthenticationController(_mockMediator, _mockTokenHandler);

            authenticationController.ConfirmEmail(userId, emailConfirmationCode);

            _mockMediator.Received().Send(Arg.Any<ConfirmEmailCommand>());
        }
        
        [Fact]
        public void GivenForgotPasswordPostMethod_WhenReceivesCorrectQuery_ThenMediatorSendMethodShouldFire()
        {
            var forgotPasswordQuery = new ForgotPasswordQuery
            {
                Email = "test@gmail.com",
            };
            
            var authenticationController = new AuthenticationController(_mockMediator, _mockTokenHandler);

            authenticationController.ForgotPassword(forgotPasswordQuery);

            _mockMediator.Received().Send(forgotPasswordQuery);
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
            
            var authenticationController = new AuthenticationController(_mockMediator, _mockTokenHandler);

            authenticationController.ResetPassword(resetPasswordQuery);

            _mockMediator.Received().Send(resetPasswordQuery);
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
            
            var authenticationController = new AuthenticationController(_mockMediator, _mockTokenHandler);

            authenticationController.ChangePassword(changePasswordCommand);

            _mockMediator.Received().Send(changePasswordCommand);
        }

        [Fact]
        public void GivenRefreshTokenPostMethod_WhenReceivesCorrectQuery_ThenShouldCallGetPrincipalFromExpiredToken()
        {
            var refreshTokenQuery = new RefreshTokenQuery
            {
                RefreshToken = "10101010",
                Token = "2020202",
                Email = "test@gmail.com"
            };
            
            var authenticationController = new AuthenticationController(_mockMediator, _mockTokenHandler);

            authenticationController.Refresh(refreshTokenQuery);
            
            _mockTokenHandler.Received().GetPrincipalFromExpiredToken(Arg.Any<string>());
        }
        
        [Fact]
        public void GivenRefreshTokenPostMethod_WhenReceivesCorrectQuery_ThenMediatorSendMethodShouldFire()
        {
            var mockClaimsPrincipal = Substitute.For<ClaimsPrincipal>();
            var refreshTokenQuery = new RefreshTokenQuery
            {
                RefreshToken = "10101010",
                Token = "2020202"
            };

            _mockTokenHandler.GetPrincipalFromExpiredToken(refreshTokenQuery.Token).ReturnsForAnyArgs(mockClaimsPrincipal);
            
            var authenticationController = new AuthenticationController(_mockMediator, _mockTokenHandler);

            authenticationController.Refresh(refreshTokenQuery);
            
            _mockMediator.Received().Send(refreshTokenQuery);
        }
    }
}