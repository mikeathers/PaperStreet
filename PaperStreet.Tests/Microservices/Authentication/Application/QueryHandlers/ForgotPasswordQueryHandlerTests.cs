using System.Threading;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using PaperStreet.Authentication.Application.CommandHandlers;
using PaperStreet.Authentication.Application.Commands;
using PaperStreet.Authentication.Application.Interfaces;
using PaperStreet.Authentication.Application.Queries;
using PaperStreet.Authentication.Application.QueryHandlers;
using PaperStreet.Authentication.Domain.Models;
using PaperStreet.Domain.Core.Bus;
using PaperStreet.Domain.Core.Events.User.Communication;
using PaperStreet.Domain.Core.Events.User.Logging;
using PaperStreet.Tests.Microservices.Authentication.Fixture;
using Xunit;

namespace PaperStreet.Tests.Microservices.Authentication.Application.QueryHandlers
{
    public class ForgotPasswordCommandHandlerTests : IClassFixture<AuthenticationFixture>
    {
        private readonly UserManager<AppUser> _mockUserManager;
        private readonly IEmailBuilder _mockEmailBuilder;
        private readonly IEventBus _mockEventBus;
        private readonly AppUser _user;
        
        public ForgotPasswordCommandHandlerTests(AuthenticationFixture fixture)
        {
            _mockUserManager = fixture.UserManager;
            _mockEmailBuilder = fixture.EmailBuilder;
            _mockEventBus = fixture.EventBus;
            _user = fixture.TestUser;
        }
        
        [Fact]
        public void GivenForgotPasswordCommandHandler_WhenReceivesCorrectCommand_ThenShouldCallUserManagerToGeneratePasswordResetToken()
        {
            const string emailAddress = "test@gmail.com";
                
            var forgotPasswordCommand = new ForgotPassword.Query
            {
                Email = emailAddress
            };

            _mockUserManager.FindByEmailAsync(emailAddress).ReturnsForAnyArgs(_user);
            
            var forgotPasswordCommandHandler = new ForgotPasswordCommandHandler(_mockUserManager, _mockEmailBuilder, _mockEventBus);
            forgotPasswordCommandHandler.Handle(forgotPasswordCommand, CancellationToken.None);

            _mockUserManager.Received().GeneratePasswordResetTokenAsync(Arg.Any<AppUser>());
        }
        
        [Fact]
        public void GivenForgotPasswordCommandHandler_WhenReceivesCorrectCommand_ThenShouldCallEmailBuilderToGetPasswordResetEmail()
        {
            const string emailAddress = "test@gmail.com";

            var forgotPasswordCommand = new ForgotPassword.Query
            {
                Email = emailAddress
            };

            _mockUserManager.FindByEmailAsync(emailAddress).ReturnsForAnyArgs(_user);
            
            var forgotPasswordCommandHandler = new ForgotPasswordCommandHandler(_mockUserManager, _mockEmailBuilder, _mockEventBus);
            forgotPasswordCommandHandler.Handle(forgotPasswordCommand, CancellationToken.None);

            _mockEmailBuilder.Received().ResetPasswordEmail(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
        }
        
        [Fact]
        public void GivenForgotPasswordCommandHandler_WhenReceivesCorrectCommand_ThenShouldPublishSendEmailEvent()
        {
            const string emailAddress = "test@gmail.com";

            var forgotPasswordCommand = new ForgotPassword.Query
            {
                Email = emailAddress
            };

            _mockUserManager.FindByEmailAsync(emailAddress).ReturnsForAnyArgs(_user);
            
            var forgotPasswordCommandHandler = new ForgotPasswordCommandHandler(_mockUserManager, _mockEmailBuilder, _mockEventBus);
            forgotPasswordCommandHandler.Handle(forgotPasswordCommand, CancellationToken.None);

            _mockEventBus.Received().Publish(Arg.Any<SendEmailEvent>());
        }
        
        [Fact]
        public void GivenForgotPasswordCommandHandler_WhenReceivesCorrectCommand_ThenShouldPublishResetPasswordRequestEvent()
        {
            const string emailAddress = "test@gmail.com";

            var forgotPasswordCommand = new ForgotPassword.Query
            {
                Email = emailAddress
            };

            _mockUserManager.FindByEmailAsync(emailAddress).ReturnsForAnyArgs(_user);
            
            var forgotPasswordCommandHandler = new ForgotPasswordCommandHandler(_mockUserManager, _mockEmailBuilder, _mockEventBus);
            forgotPasswordCommandHandler.Handle(forgotPasswordCommand, CancellationToken.None);

            _mockEventBus.Received().Publish(Arg.Any<ResetPasswordRequestEvent>());
        }
    }
}