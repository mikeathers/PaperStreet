using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using PaperStreet.Authentication.Application.CommandHandlers;
using PaperStreet.Authentication.Application.Commands;
using PaperStreet.Domain.Core.Events.User;
using PaperStreet.Domain.Core.Models;
using PaperStreet.Tests.Microservices.Authentication.Fixture;
using Xunit;

namespace PaperStreet.Tests.Microservices.Authentication.Application.CommandHandlers
{
    public class ConfirmEmailCommandHandlerTests : IClassFixture<AuthenticationFixture>
    {
        private readonly AuthenticationFixture _fixture;

        public ConfirmEmailCommandHandlerTests(AuthenticationFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GivenConfirmEmailCommandHandler_WhenReceivesCorrectCommand_ThenShouldConfirmUserAccountAndReturnUser()
        {
            const string emailConfirmationToken = "101010101";
            const string userId = "1092093dk0230-2";

            var confirmEmailCommand = new ConfirmEmail.Command
            {
                UserId = userId,
                EmailConfirmationCode = emailConfirmationToken
            };
            
            var userToConfirm = _fixture.TestUser;
            var mockUserManager = _fixture.UserManager;
            var mockJwtGenerator = _fixture.JwtGenerator;
            var mockEventBus = _fixture.EventBus;
                
            mockUserManager.ConfirmEmailAsync(userToConfirm, emailConfirmationToken)
                            .ReturnsForAnyArgs(Task.FromResult(IdentityResult.Success));
            
            mockUserManager.FindByIdAsync(userId).ReturnsForAnyArgs(userToConfirm);

            userToConfirm.EmailConfirmed = true;
            
            var confirmEmailCommandHandler = new ConfirmEmailCommandHandler(mockUserManager, mockJwtGenerator, mockEventBus);

            var confirmedUser = await confirmEmailCommandHandler.Handle(confirmEmailCommand, CancellationToken.None);
            
            Assert.NotNull(confirmedUser);
            Assert.True(confirmedUser.EmailConfirmed);
        }
        
        [Fact]
        public async Task GivenConfirmEmailCommandHandler_WhenUserIsNotFound_ThenShouldThrowRestException()
        {
            const string emailConfirmationToken = "101010101";
            const string userId = "1092093dk0230-2";

            var confirmEmailCommand = new ConfirmEmail.Command
            {
                UserId = userId,
                EmailConfirmationCode = emailConfirmationToken
            };
            
            var mockUserManager = _fixture.UserManager;
            var mockJwtGenerator = _fixture.JwtGenerator;
            var mockEventBus = _fixture.EventBus;
            
            mockUserManager.FindByIdAsync(userId).ReturnsNullForAnyArgs();

            var confirmEmailCommandHandler = new ConfirmEmailCommandHandler(mockUserManager, mockJwtGenerator, mockEventBus);

            await Assert.ThrowsAsync<RestException>(() =>
                confirmEmailCommandHandler.Handle(confirmEmailCommand, CancellationToken.None));
            
        }
        
        [Fact]
        public async Task GivenConfirmEmailCommandHandler_WhenEmailConfirmed_ThenShouldPublishEmailConfirmedEvent()
        {
            const string emailConfirmationToken = "101010101";
            const string userId = "1092093dk0230-2";

            var confirmEmailCommand = new ConfirmEmail.Command
            {
                UserId = userId,
                EmailConfirmationCode = emailConfirmationToken
            };
            
            var userToConfirm = _fixture.TestUser;
            var mockUserManager = _fixture.UserManager;
            var mockJwtGenerator = _fixture.JwtGenerator;
            var mockEventBus = _fixture.EventBus;
                
            mockUserManager.ConfirmEmailAsync(userToConfirm, emailConfirmationToken)
                .ReturnsForAnyArgs(Task.FromResult(IdentityResult.Success));
            
            mockUserManager.FindByIdAsync(userId).ReturnsForAnyArgs(userToConfirm);

            userToConfirm.EmailConfirmed = true;
            
            var confirmEmailCommandHandler = new ConfirmEmailCommandHandler(mockUserManager, mockJwtGenerator, mockEventBus);

            await confirmEmailCommandHandler.Handle(confirmEmailCommand, CancellationToken.None);
            
            mockEventBus.Received().Publish(Arg.Any<EmailConfirmedEvent>());
        }
    }
}