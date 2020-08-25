using System.Collections.Generic;
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
    public class ChangePasswordCommandHandlerTests : IClassFixture<AuthenticationFixture>
    {
        private readonly UserManager<AppUser> _mockUserManager;
        private readonly IEmailBuilder _mockEmailBuilder;
        private readonly IEventBus _mockEventBus;
        private readonly IFailedIdentityResult _mockFailedIdentityResult;
        private readonly AppUser _user;
        private readonly ChangePasswordCommand _command;

        public ChangePasswordCommandHandlerTests(AuthenticationFixture fixture)
        {
            _mockUserManager = fixture.UserManager;
            _mockEmailBuilder = fixture.EmailBuilder;
            _mockEventBus = fixture.EventBus;
            _mockFailedIdentityResult = fixture.FailedIdentityResult;
            _user = fixture.TestUser;

            _command = new ChangePasswordCommand
            {
                Email = "test@gmail.com",
                CurrentPassword = "Password1!",
                NewPassword = "Password123!"
            };
        }
        
        [Fact]
        public async Task
            GivenChangePasswordCommandHandler_WhenReceivesCorrectCommand_ThenShouldCallUserManagerToFindUser()
        {
            _mockUserManager.FindByEmailAsync(_command.Email).ReturnsForAnyArgs(_user);
            _mockUserManager.ChangePasswordAsync(_user, _command.CurrentPassword, _command.NewPassword)
                .ReturnsForAnyArgs(Task.FromResult(IdentityResult.Success));
            
            var changePasswordCommandHandler =
                new ChangePasswordCommandHandler(_mockUserManager, _mockEmailBuilder, _mockEventBus,
                    _mockFailedIdentityResult);

            await changePasswordCommandHandler.Handle(_command, CancellationToken.None);

            await _mockUserManager.Received().FindByEmailAsync(_command.Email);
        }
        
        [Fact]
        public async Task
            GivenChangePasswordCommandHandler_WhenUserNotFound_ThenShouldThrowRestException()
        {
            _mockUserManager.FindByEmailAsync(_command.Email).ReturnsNullForAnyArgs();
            
            var changePasswordCommandHandler =
                new ChangePasswordCommandHandler(_mockUserManager, _mockEmailBuilder, _mockEventBus,
                    _mockFailedIdentityResult);

            await Assert.ThrowsAsync<RestException>(() =>
                changePasswordCommandHandler.Handle(_command, CancellationToken.None));
        }
        
        [Fact]
        public async Task
            GivenChangePasswordCommandHandler_WhenReceivesCorrectCommand_ThenShouldCallUserManagerToChangePassword()
        {
            _mockUserManager.FindByEmailAsync(_command.Email).ReturnsForAnyArgs(_user);
            _mockUserManager.ChangePasswordAsync(_user, _command.CurrentPassword, _command.NewPassword)
                .ReturnsForAnyArgs(Task.FromResult(IdentityResult.Success));
            
            var changePasswordCommandHandler = 
                new ChangePasswordCommandHandler(_mockUserManager, _mockEmailBuilder, _mockEventBus,
                    _mockFailedIdentityResult);

            await changePasswordCommandHandler.Handle(_command, CancellationToken.None);

            await _mockUserManager.Received().ChangePasswordAsync(_user, _command.CurrentPassword, _command.NewPassword);
        }
        
        [Fact]
        public async Task
            GivenChangePasswordCommandHandler_WhenPasswordChangeFails_ThenShouldCallFailedIdentityResult()
        {
            _mockUserManager.FindByEmailAsync(_command.Email).ReturnsForAnyArgs(_user);
            _mockUserManager.ChangePasswordAsync(_user, _command.CurrentPassword, _command.NewPassword)
                .ReturnsForAnyArgs(Task.FromResult(IdentityResult.Failed()));
            
            var changePasswordCommandHandler = 
                new ChangePasswordCommandHandler(_mockUserManager, _mockEmailBuilder, _mockEventBus,
                    _mockFailedIdentityResult);

            
            await changePasswordCommandHandler.Handle(_command, CancellationToken.None);
            
            _mockFailedIdentityResult.Received()
                .Handle(Arg.Any<AppUser>(), Arg.Any<List<IdentityError>>(), Arg.Any<string>());
        }

        [Fact]
        public async Task
            GivenChangePasswordCommandHandler_WhenPasswordChanged_ThenShouldCallEmailBuilder()
        {
            _mockUserManager.FindByEmailAsync(_command.Email).ReturnsForAnyArgs(_user);
            _mockUserManager.ChangePasswordAsync(_user, _command.CurrentPassword, _command.NewPassword)
                .ReturnsForAnyArgs(Task.FromResult(IdentityResult.Success));
            
            var changePasswordCommandHandler = 
                new ChangePasswordCommandHandler(_mockUserManager, _mockEmailBuilder, _mockEventBus,
                    _mockFailedIdentityResult);

            await changePasswordCommandHandler.Handle(_command, CancellationToken.None);

            _mockEmailBuilder.Received().PasswordChangedEmail(Arg.Any<string>());
        }
        
        [Fact]
        public async Task
            GivenChangePasswordCommandHandler_WhenPasswordChanged_ThenShouldPublishSendEmailEvent()
        {
            _mockUserManager.FindByEmailAsync(_command.Email).ReturnsForAnyArgs(_user);
            _mockUserManager.ChangePasswordAsync(_user, _command.CurrentPassword, _command.NewPassword)
                .ReturnsForAnyArgs(Task.FromResult(IdentityResult.Success));
            
            var changePasswordCommandHandler = 
                new ChangePasswordCommandHandler(_mockUserManager, _mockEmailBuilder, _mockEventBus,
                    _mockFailedIdentityResult);

            await changePasswordCommandHandler.Handle(_command, CancellationToken.None);

            _mockEventBus.Received().Publish(Arg.Any<SendEmailEvent>());
        }
        
        [Fact]
        public async Task
            GivenChangePasswordCommandHandler_WhenPasswordChanged_ThenShouldPasswordChangedEvent()
        {
            _mockUserManager.FindByEmailAsync(_command.Email).ReturnsForAnyArgs(_user);
            _mockUserManager.ChangePasswordAsync(_user, _command.CurrentPassword, _command.NewPassword)
                .ReturnsForAnyArgs(Task.FromResult(IdentityResult.Success));
            
            var changePasswordCommandHandler = 
                new ChangePasswordCommandHandler(_mockUserManager, _mockEmailBuilder, _mockEventBus,
                    _mockFailedIdentityResult);

            await changePasswordCommandHandler.Handle(_command, CancellationToken.None);

            _mockEventBus.Received().Publish(Arg.Any<PasswordChangedEvent>());
        }
    }
}