using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using PaperStreet.Authentication.Application.CommandHandlers;
using PaperStreet.Authentication.Application.Commands;
using PaperStreet.Authentication.Application.Interfaces;
using PaperStreet.Authentication.Domain.Models;
using PaperStreet.Domain.Core.Bus;
using PaperStreet.Domain.Core.Events.User.Communication;
using PaperStreet.Domain.Core.Events.User.Logging;
using PaperStreet.Domain.Core.Models;
using PaperStreet.Tests.Microservices.Authentication.Fixture;
using Xunit;

namespace PaperStreet.Tests.Microservices.Authentication.Application.CommandHandlers
{
    public class ResetPasswordCommandHandlerTests : IClassFixture<AuthenticationFixture>
    {
        private readonly UserManager<AppUser> _mockUserManager;
        private readonly IEventBus _mockEventBus;
        private readonly IEmailBuilder _mockEmailBuilder;
        private readonly ResetPassword.Command _command;
        private readonly AppUser _user;
        
        public ResetPasswordCommandHandlerTests(AuthenticationFixture fixture)
        {
            _mockEventBus = fixture.EventBus;
            _mockUserManager = fixture.UserManager;
            _mockEmailBuilder = fixture.EmailBuilder;
            _user = fixture.TestUser;

            _command = new ResetPassword.Command
            {
                Email = "test@gmail.com",
                NewPassword = "Password123!",
                ResetPasswordToken = "10101010"
            };
        }
        
        [Fact]
        public async Task GivenResetPasswordCommandHandler_WhenCorrectQueryReceived_ThenShouldUseUserManagerToFindUser()
        {
            _mockUserManager.FindByEmailAsync(_command.Email).ReturnsForAnyArgs(_user);
            
            var resetPasswordCommandHandler = new ResetPasswordCommandHandler(_mockUserManager, _mockEventBus, _mockEmailBuilder);

            await resetPasswordCommandHandler.Handle(_command, CancellationToken.None);

            await _mockUserManager.ReceivedWithAnyArgs().FindByEmailAsync(_command.Email);
        }
        
        [Fact]
        public async Task GivenResetPasswordCommandHandler_WhenUserNotFound_ThenShouldThrowRestException()
        {
            _mockUserManager.FindByEmailAsync(_command.Email).ReturnsNullForAnyArgs();
            
            var resetPasswordCommandHandler = new ResetPasswordCommandHandler(_mockUserManager, _mockEventBus, _mockEmailBuilder);

            await Assert.ThrowsAsync<RestException>(() =>
                resetPasswordCommandHandler.Handle(_command, CancellationToken.None));
        }
        
        [Fact]
        public async Task GivenResetPasswordCommandHandler_WhenCorrectQueryReceived_ThenShouldUseUserManagerToResetPassword()
        {
            _mockUserManager.FindByEmailAsync(_command.Email).ReturnsForAnyArgs(_user);
            _mockUserManager.ResetPasswordAsync(_user, _command.ResetPasswordToken, _command.NewPassword)
                .ReturnsForAnyArgs(Task.FromResult(IdentityResult.Success));
            
            var resetPasswordCommandHandler = new ResetPasswordCommandHandler(_mockUserManager, _mockEventBus, _mockEmailBuilder);

            await resetPasswordCommandHandler.Handle(_command, CancellationToken.None);

            await _mockUserManager.Received().ResetPasswordAsync(_user, _command.ResetPasswordToken, _command.NewPassword);
        }
        
        [Fact]
        public async Task GivenResetPasswordCommandHandler_WhenPasswordHasBeenReset_ThenShouldCallEmailBuilder()
        {
            _mockUserManager.FindByEmailAsync(_command.Email).ReturnsForAnyArgs(_user);
            _mockUserManager.ResetPasswordAsync(_user, _command.ResetPasswordToken, _command.NewPassword)
                .ReturnsForAnyArgs(Task.FromResult(IdentityResult.Success));
            
            var resetPasswordCommandHandler = new ResetPasswordCommandHandler(_mockUserManager, _mockEventBus, _mockEmailBuilder);

            await resetPasswordCommandHandler.Handle(_command, CancellationToken.None);

            _mockEmailBuilder.Received().PasswordChangedEmail(Arg.Any<string>());
        }
        
        [Fact]
        public async Task GivenResetPasswordCommandHandler_WhenPasswordHasBeenReset_ThenShouldPublishPasswordChangedEvent()
        {
            _mockUserManager.FindByEmailAsync(_command.Email).ReturnsForAnyArgs(_user);
            _mockUserManager.ResetPasswordAsync(_user, _command.ResetPasswordToken, _command.NewPassword)
                .ReturnsForAnyArgs(Task.FromResult(IdentityResult.Success));
            
            var resetPasswordCommandHandler = new ResetPasswordCommandHandler(_mockUserManager, _mockEventBus, _mockEmailBuilder);

            await resetPasswordCommandHandler.Handle(_command, CancellationToken.None);

            _mockEventBus.Received().Publish(Arg.Any<PasswordChangedEvent>());
        }
        
        [Fact]
        public async Task GivenResetPasswordCommandHandler_WhenPasswordHasBeenReset_ThenShouldPublishSendEmailEvent()
        {
            _mockUserManager.FindByEmailAsync(_command.Email).ReturnsForAnyArgs(_user);
            _mockUserManager.ResetPasswordAsync(_user, _command.ResetPasswordToken, _command.NewPassword)
                .ReturnsForAnyArgs(Task.FromResult(IdentityResult.Success));
            
            var resetPasswordCommandHandler = new ResetPasswordCommandHandler(_mockUserManager, _mockEventBus, _mockEmailBuilder);

            await resetPasswordCommandHandler.Handle(_command, CancellationToken.None);

            _mockEventBus.Received().Publish(Arg.Any<SendEmailEvent>());
        }
    }
}