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
using PaperStreet.Domain.Core.Events.User;
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
        private readonly ConfirmEmail.Command _command;
        private readonly AppUser _user;
        
        private static string EmailConfirmationToken => "101010101";
        private static string UserId => "1092093dk0230-2";

        public ConfirmEmailCommandHandlerTests(AuthenticationFixture fixture)
        {
            var localFixture = fixture;
            _mockUserManager = localFixture.UserManager;
            _mockJwtGenerator = localFixture.JwtGenerator;
            _mockEventBus = localFixture.EventBus;
            _user = localFixture.TestUser;
            _user.EmailConfirmed = true;
            
            _command = new ConfirmEmail.Command
            {
                UserId = "101010101",
                EmailConfirmationCode = "1092093dk0230-2"
            };
        }

        [Fact]
        public async Task GivenConfirmEmailCommandHandler_WhenReceivesCorrectCommand_ThenShouldConfirmUserAccountAndReturnUser()
        {
            _mockUserManager.ConfirmEmailAsync(_user, EmailConfirmationToken)
                            .ReturnsForAnyArgs(Task.FromResult(IdentityResult.Success));
            
            _mockUserManager.FindByIdAsync(UserId).ReturnsForAnyArgs(_user);

            var confirmEmailCommandHandler = new ConfirmEmailCommandHandler(_mockUserManager, _mockJwtGenerator, _mockEventBus);

            var confirmedUser = await confirmEmailCommandHandler.Handle(_command, CancellationToken.None);
            
            Assert.NotNull(confirmedUser);
            Assert.True(confirmedUser.EmailConfirmed);
        }
        
        [Fact]
        public async Task GivenConfirmEmailCommandHandler_WhenUserIsNotFound_ThenShouldThrowRestException()
        {
            _mockUserManager.FindByIdAsync(UserId).ReturnsNullForAnyArgs();

            var confirmEmailCommandHandler = new ConfirmEmailCommandHandler(_mockUserManager, _mockJwtGenerator, _mockEventBus);

            await Assert.ThrowsAsync<RestException>(() =>
                confirmEmailCommandHandler.Handle(_command, CancellationToken.None));
        }
        
        [Fact]
        public async Task GivenConfirmEmailCommandHandler_WhenEmailConfirmed_ThenShouldPublishEmailConfirmedEvent()
        {
            _mockUserManager.ConfirmEmailAsync(_user, EmailConfirmationToken)
                .ReturnsForAnyArgs(Task.FromResult(IdentityResult.Success));
            
            _mockUserManager.FindByIdAsync(UserId).ReturnsForAnyArgs(_user);

            var confirmEmailCommandHandler = new ConfirmEmailCommandHandler(_mockUserManager, _mockJwtGenerator, _mockEventBus);

            await confirmEmailCommandHandler.Handle(_command, CancellationToken.None);
            
            _mockEventBus.Received().Publish(Arg.Any<EmailConfirmedEvent>());
        }
    }
}