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
using PaperStreet.Domain.Core.Events.User.Logging;
using PaperStreet.Domain.Core.Models;
using PaperStreet.Tests.Microservices.Authentication.Fixture;
using Xunit;

namespace PaperStreet.Tests.Microservices.Authentication.Application.CommandHandlers
{
    public class ConfirmEmailCommandHandlerTests : IClassFixture<AuthenticationFixture>
    {
        private readonly UserManager<AppUser> _mockUserManager;
        private readonly IJwtGenerator _mockJwtGenerator;
        private readonly IEventBus _mockEventBus;
        private readonly IFailedIdentityResult _mockFailedIdentityResult;
        private readonly ConfirmEmail.Command _command;
        private readonly AppUser _user;

        private readonly string _emailConfirmationToken;
        private readonly string _email;

        public ConfirmEmailCommandHandlerTests(AuthenticationFixture fixture)
        {
            _mockUserManager = fixture.UserManager;
            _mockJwtGenerator = fixture.JwtGenerator;
            _mockEventBus = fixture.EventBus;
            _mockFailedIdentityResult = fixture.FailedIdentityResult;
            _user = fixture.TestUser;
            _user.EmailConfirmed = true;

            _email = "test@gmail.com";
            _emailConfirmationToken = "1010d1d120e";
            
            _command = new ConfirmEmail.Command
            {
                Email = _email,
                EmailConfirmationCode = "1092093dk0230-2"
            };
        }

        [Fact]
        public async Task
            GivenConfirmEmailCommandHandler_WhenReceivesCorrectCommand_ThenShouldCallUserManagerToFindByEmail()
        {
            _mockUserManager.ConfirmEmailAsync(_user, _emailConfirmationToken)
                .ReturnsForAnyArgs(Task.FromResult(IdentityResult.Success));
            
            _mockUserManager.FindByEmailAsync(_email).ReturnsForAnyArgs(_user);

            var confirmEmailCommandHandler = new ConfirmEmailCommandHandler(_mockUserManager, _mockJwtGenerator,
                _mockEventBus, _mockFailedIdentityResult);

            await confirmEmailCommandHandler.Handle(_command, CancellationToken.None);

            await _mockUserManager.Received().FindByEmailAsync(_email);
        }
        
        [Fact]
        public async Task GivenConfirmEmailCommandHandler_WhenUserIsNotFound_ThenShouldThrowRestException()
        {
            _mockUserManager.FindByEmailAsync(_email).ReturnsNullForAnyArgs();

            var confirmEmailCommandHandler = new ConfirmEmailCommandHandler(_mockUserManager, _mockJwtGenerator,
                _mockEventBus, _mockFailedIdentityResult);

            await Assert.ThrowsAsync<RestException>(() =>
                confirmEmailCommandHandler.Handle(_command, CancellationToken.None));
        }
        
        [Fact]
        public async Task
            GivenConfirmEmailCommandHandler_WhenEmailConfirmFails_ThenShouldCallFailedIdentityResult()
        {
            _mockUserManager.ConfirmEmailAsync(_user, _emailConfirmationToken)
                .ReturnsForAnyArgs(Task.FromResult(IdentityResult.Failed()));
            
            _mockUserManager.FindByEmailAsync(_email).ReturnsForAnyArgs(_user);

            var confirmEmailCommandHandler = new ConfirmEmailCommandHandler(_mockUserManager, _mockJwtGenerator,
                _mockEventBus, _mockFailedIdentityResult);

            await confirmEmailCommandHandler.Handle(_command, CancellationToken.None);

            _mockFailedIdentityResult.Received()
                .Handle(Arg.Any<AppUser>(), Arg.Any<List<IdentityError>>(), Arg.Any<string>());
        }
        
        [Fact]
        public async Task
            GivenConfirmEmailCommandHandler_WhenEmailConfirmed_ThenShouldCallJwtGenerator()
        {
            _mockUserManager.ConfirmEmailAsync(_user, _emailConfirmationToken)
                .ReturnsForAnyArgs(Task.FromResult(IdentityResult.Success));
            
            _mockUserManager.FindByEmailAsync(_email).ReturnsForAnyArgs(_user);

            var confirmEmailCommandHandler = new ConfirmEmailCommandHandler(_mockUserManager, _mockJwtGenerator,
                _mockEventBus, _mockFailedIdentityResult);

            await confirmEmailCommandHandler.Handle(_command, CancellationToken.None);

            _mockJwtGenerator.Received().CreateToken(_user);
            _mockJwtGenerator.Received().GenerateRefreshToken();
        }
        
        [Fact]
        public async Task
            GivenConfirmEmailCommandHandler_WhenEmailConfirmed_ThenShouldCallUserManagerToUpdateUser()
        {
            _mockUserManager.ConfirmEmailAsync(_user, _emailConfirmationToken)
                .ReturnsForAnyArgs(Task.FromResult(IdentityResult.Success));
            
            _mockUserManager.FindByEmailAsync(_email).ReturnsForAnyArgs(_user);

            var confirmEmailCommandHandler = new ConfirmEmailCommandHandler(_mockUserManager, _mockJwtGenerator,
                _mockEventBus, _mockFailedIdentityResult);

            await confirmEmailCommandHandler.Handle(_command, CancellationToken.None);

            await _mockUserManager.Received().UpdateAsync(_user);
        }

        [Fact]
        public async Task GivenConfirmEmailCommandHandler_WhenEmailConfirmed_ThenShouldPublishAuthenticationLogEvent()
        {
            _mockUserManager.ConfirmEmailAsync(_user, _emailConfirmationToken)
                .ReturnsForAnyArgs(Task.FromResult(IdentityResult.Success));
            
            _mockUserManager.FindByEmailAsync(_email).ReturnsForAnyArgs(_user);

            var confirmEmailCommandHandler = new ConfirmEmailCommandHandler(_mockUserManager, _mockJwtGenerator,
                _mockEventBus, _mockFailedIdentityResult);

            await confirmEmailCommandHandler.Handle(_command, CancellationToken.None);
            
            _mockEventBus.Received().Publish(Arg.Any<AuthenticationLogEvent>());
        }
        
        [Fact]
        public async Task
            GivenConfirmEmailCommandHandler_WhenReceivesCorrectCommand_ThenShouldConfirmUserAccountAndReturnUser()
        {
            _mockUserManager.ConfirmEmailAsync(_user, _emailConfirmationToken)
                .ReturnsForAnyArgs(Task.FromResult(IdentityResult.Success));
            
            _mockUserManager.FindByEmailAsync(_email).ReturnsForAnyArgs(_user);

            var confirmEmailCommandHandler = new ConfirmEmailCommandHandler(_mockUserManager, _mockJwtGenerator,
                _mockEventBus, _mockFailedIdentityResult);

            var confirmedUser = await confirmEmailCommandHandler.Handle(_command, CancellationToken.None);
            
            Assert.NotNull(confirmedUser);
            Assert.True(confirmedUser.EmailConfirmed);
        }
    }
}