using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using PaperStreet.Authentication.Application.CommandHandlers;
using PaperStreet.Authentication.Application.Commands;
using PaperStreet.Authentication.Data.Context;
using PaperStreet.Domain.Core.Events.User;
using PaperStreet.Domain.Core.Models;
using PaperStreet.Tests.Microservices.Authentication.Fixture;
using PaperStreet.Tests.Microservices.Authentication.SeedData;
using TestSupport.EfHelpers;
using Xunit;

namespace PaperStreet.Tests.Microservices.Authentication.Application.CommandHandlers
{
    public class RegisterUserCommandHandlerTests : IClassFixture<AuthenticationFixture>
    {
        private readonly AuthenticationFixture _fixture;

        public RegisterUserCommandHandlerTests(AuthenticationFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GivenRegisterUserCommandHandler_WhenUserEmailAlreadyExists_ThenThrowsBadRequestException()
        {
            var options = SqliteInMemory.CreateOptions<AuthenticationDbContext>();
            await using var context = new AuthenticationDbContext(options);
            {
                await context.Database.EnsureCreatedAsync();
                context.SeedSingleUserData();;

                var registerCommand = new RegisterUser.Command
                {
                    DisplayName = "Test User",
                    Email = "test@gmail.com",
                    Password = "password123"
                };

                var mockUserManager = _fixture.UserManager;
                var mockJwtGenerator = _fixture.JwtGenerator;
                var mockEventBus = _fixture.EventBus;
                
                var registerCommandHandler = new RegisterUserCommandHandler(context, mockUserManager, mockJwtGenerator, mockEventBus);

                await Assert.ThrowsAsync<RestException>(() =>
                    registerCommandHandler.Handle(registerCommand, CancellationToken.None));
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

                var registerCommand = new RegisterUser.Command
                {
                    DisplayName = "Test User",
                    Email = "testuser@gmail.com",
                    Password = "password123"
                };

                var newUser = _fixture.TestUser;
                var mockUserManager = _fixture.UserManager;    
                var mockJwtGenerator = _fixture.JwtGenerator;
                var mockEventBus = _fixture.EventBus;
                
                mockUserManager.CreateAsync(newUser, registerCommand.Password)
                    .ReturnsForAnyArgs(Task.FromResult(IdentityResult.Success));

                var registerCommandHandler = new RegisterUserCommandHandler(context, mockUserManager, mockJwtGenerator, mockEventBus);
                var registeredUser = await registerCommandHandler.Handle(registerCommand, CancellationToken.None);
                
                Assert.NotNull(registeredUser);
                Assert.Equal(registeredUser.Email, newUser.Email);
            }
        }
        
        [Fact]
        public async Task GivenRegisterUserCommandHandler_WhenNewUserHasRegistered_ThenUserRegisteredEventShouldBePublished()
        {
            var options = SqliteInMemory.CreateOptions<AuthenticationDbContext>();
            await using var context = new AuthenticationDbContext(options);
            {
                await context.Database.EnsureCreatedAsync();
                context.SeedSingleUserData();

                var registerCommand = new RegisterUser.Command
                {
                    DisplayName = "Test User",
                    Email = "testuser@gmail.com",
                    Password = "password123"
                };

                var createdUser = _fixture.TestUser;
                var mockUserManager = _fixture.UserManager;    
                var mockJwtGenerator = _fixture.JwtGenerator;
                var mockEventBus = _fixture.EventBus;
                
                mockUserManager.CreateAsync(createdUser, registerCommand.Password)
                    .ReturnsForAnyArgs(Task.FromResult(IdentityResult.Success));

                var registerCommandHandler = new RegisterUserCommandHandler(context, mockUserManager, mockJwtGenerator, mockEventBus);
                await registerCommandHandler.Handle(registerCommand, CancellationToken.None);
                
                mockEventBus.Received().Publish(Arg.Any<UserRegisteredEvent>());
            }
        }
    }
}