using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using PaperStreet.Authentication.Application.CommandHandlers;
using PaperStreet.Authentication.Application.Commands;
using PaperStreet.Authentication.Application.Interfaces;
using PaperStreet.Authentication.Data.Context;
using PaperStreet.Authentication.Domain.Models;
using PaperStreet.Domain.Core.Bus;
using PaperStreet.Domain.Core.Events.User.Logging;
using PaperStreet.Domain.Core.Models;
using PaperStreet.Tests.Microservices.Authentication.Fixture;
using PaperStreet.Tests.Microservices.Authentication.SeedData;
using TestSupport.EfHelpers;
using Xunit;

namespace PaperStreet.Tests.Microservices.Authentication.Application.CommandHandlers
{
    public class RegisterUserCommandHandlerTests : IClassFixture<AuthenticationFixture>
    {
        private readonly UserManager<AppUser> _mockUserManager;
        private readonly IJwtGenerator _mockJwtGenerator;
        private readonly IEventBus _mockEventBus;
        private readonly IUserConfirmationEmail _mockUserConfirmationEmail;
        private readonly RegisterUser.Command _command;
        private readonly AppUser _user;
            
        public RegisterUserCommandHandlerTests(AuthenticationFixture fixture)
        {
            _mockUserConfirmationEmail = fixture.UserConfirmationEmail;
            _mockUserManager = fixture.UserManager;
            _mockJwtGenerator = fixture.JwtGenerator;
            _mockEventBus = fixture.EventBus;
            _user = fixture.TestUser;
            
            _command = new RegisterUser.Command
            {
                FirstName = "Test User",
                Email = "testuser@gmail.com",
                Password = "password123"
            };
        }

        [Fact]
        public async Task GivenRegisterUserCommandHandler_WhenUserEmailAlreadyExists_ThenThrowsBadRequestException()
        {
            var options = SqliteInMemory.CreateOptions<AuthenticationDbContext>();
            await using var context = new AuthenticationDbContext(options);
            {
                await context.Database.EnsureCreatedAsync();
                context.SeedSingleUserData();;

                var invalidCommand = new RegisterUser.Command
                {
                    FirstName = "Test User",
                    Email = "test@gmail.com",
                    Password = "password123"
                };

                var registerCommandHandler =
                    new RegisterUserCommandHandler(context, _mockUserManager, _mockJwtGenerator, _mockEventBus, _mockUserConfirmationEmail);

                await Assert.ThrowsAsync<RestException>(() =>
                    registerCommandHandler.Handle(invalidCommand, CancellationToken.None));
            }
        }
        
        [Fact]
        public async Task GivenRegisterUserCommandHandler_WhenNewUserDetailsAreProvided_ThenCreatesNewUser()
        {
            var options = SqliteInMemory.CreateOptions<AuthenticationDbContext>();
            await using var context = new AuthenticationDbContext(options);
            {
                await context.Database.EnsureCreatedAsync();
                context.SeedSingleUserData();

                _mockUserManager.CreateAsync(_user, _command.Password)
                    .ReturnsForAnyArgs(Task.FromResult(IdentityResult.Success));

                var registerCommandHandler = 
                    new RegisterUserCommandHandler(context, _mockUserManager, _mockJwtGenerator, _mockEventBus, _mockUserConfirmationEmail);
                
                var registeredUser = await registerCommandHandler.Handle(_command, CancellationToken.None);
                
                Assert.NotNull(registeredUser);
                Assert.Equal(registeredUser.Email, _user.Email);
            }
        }
        
        [Fact]
        public async Task GivenRegisterUserCommandHandler_WhenNewUserHasRegistered_ThenShouldPublishAuthenticationLogEvent()
        {
            var options = SqliteInMemory.CreateOptions<AuthenticationDbContext>();
            await using var context = new AuthenticationDbContext(options);
            {
                await context.Database.EnsureCreatedAsync();
                context.SeedSingleUserData();

                _mockUserManager.CreateAsync(_user, _command.Password)
                    .ReturnsForAnyArgs(Task.FromResult(IdentityResult.Success));

                var registerCommandHandler = 
                    new RegisterUserCommandHandler(context, _mockUserManager, _mockJwtGenerator, _mockEventBus, _mockUserConfirmationEmail);
                
                await registerCommandHandler.Handle(_command, CancellationToken.None);
                
                _mockEventBus.Received().Publish(Arg.Any<AuthenticationLogEvent>());
            }
        }
        
        [Fact]
        public async Task GivenRegisterUserCommandHandler_WhenNewUserHasRegistered_ThenUserConfirmationEmailShouldBeCalled()
        {
            var options = SqliteInMemory.CreateOptions<AuthenticationDbContext>();
            await using var context = new AuthenticationDbContext(options);
            {
                await context.Database.EnsureCreatedAsync();
                context.SeedSingleUserData();
                
                _mockUserManager.CreateAsync(_user, _command.Password)
                    .ReturnsForAnyArgs(Task.FromResult(IdentityResult.Success));

                var registerCommandHandler =
                    new RegisterUserCommandHandler(context, _mockUserManager, _mockJwtGenerator, _mockEventBus,
                        _mockUserConfirmationEmail);

                await registerCommandHandler.Handle(_command, CancellationToken.None);

                await _mockUserConfirmationEmail.Received().Send(Arg.Any<AppUser>());
            }
        }
    }
}