using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using PaperStreet.Authentication.Application.CommandHandlers;
using PaperStreet.Authentication.Data.Context;
using PaperStreet.Domain.Core.Events.User;
using PaperStreet.Domain.Core.Models;
using PaperStreet.Tests.Microservices.Authentication.Fixture;
using PaperStreet.Tests.Microservices.Authentication.SeedData;
using TestSupport.EfHelpers;
using Xunit;

namespace PaperStreet.Tests.Microservices.Authentication.Application.CommandHandlers
{
    public class RegisterCommandHandler : IClassFixture<AuthenticationFixture>
    {
        private readonly AuthenticationFixture _fixture;

        public RegisterCommandHandler(AuthenticationFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GivenRegisterCommandHandler_WhenUserEmailAlreadyExists_ThenThrowsBadRequestException()
        {
            var options = SqliteInMemory.CreateOptions<AuthenticationDbContext>();
            await using var context = new AuthenticationDbContext(options);
            {
                await context.Database.EnsureCreatedAsync();
                context.SeedUserData();

                var registerCommand = new PaperStreet.Authentication.Application.Commands.RegisterUser.Command
                {
                    DisplayName = "Test User",
                    Username = "test@gmail.com",
                    Email = "test@gmail.com",
                    Password = "password123"
                };

                var mockUserManager = _fixture.UserManager;
                var mockJwtGenerator = _fixture.JwtGenerator;
                var mockEventBus = _fixture.EventBus;
                
                var registerCommandHandler = new RegisterUser(context, mockUserManager, mockJwtGenerator, mockEventBus);

                await Assert.ThrowsAsync<RestException>(() =>
                    registerCommandHandler.Handle(registerCommand, CancellationToken.None));
            }
        }
        
        [Fact]
        public async Task GivenRegisterCommandHandler_WhenUserUserNameAlreadyExists_ThenThrowsBadRequestException()
        {
            var options = SqliteInMemory.CreateOptions<AuthenticationDbContext>();
            await using var context = new AuthenticationDbContext(options);
            {
                await context.Database.EnsureCreatedAsync();
                context.SeedUserData();

                var registerCommand = new PaperStreet.Authentication.Application.Commands.RegisterUser.Command
                {
                    DisplayName = "Test User",
                    Username = "test@gmail.com",
                    Email = "test@gmail.com1",
                    Password = "password123"
                };

                var mockUserManager = _fixture.UserManager;
                var mockJwtGenerator = _fixture.JwtGenerator;
                var mockEventBus = _fixture.EventBus;
                
                var registerCommandHandler = new RegisterUser(context, mockUserManager, mockJwtGenerator, mockEventBus);

                await Assert.ThrowsAsync<RestException>(() =>
                    registerCommandHandler.Handle(registerCommand, CancellationToken.None));
            }
        }
        
        [Fact]
        public async Task GivenRegisterCommandHandler_WhenNewUserDetailsAreProvided_ThenCreatesNewUser()
        {
            var options = SqliteInMemory.CreateOptions<AuthenticationDbContext>();
            await using var context = new AuthenticationDbContext(options);
            {
                await context.Database.EnsureCreatedAsync();
                context.SeedUserData();

                var registerCommand = new PaperStreet.Authentication.Application.Commands.RegisterUser.Command
                {
                    DisplayName = "Test User",
                    Username = "testuser@gmail.com",
                    Email = "testuser@gmail.com",
                    Password = "password123"
                };

                var createdUser = _fixture.TestUser;
                var mockUserManager = _fixture.UserManager;    
                var mockJwtGenerator = _fixture.JwtGenerator;
                var mockEventBus = _fixture.EventBus;
                
                mockUserManager.CreateAsync(createdUser, registerCommand.Password)
                    .ReturnsForAnyArgs(Task.FromResult(IdentityResult.Success));

                var registerCommandHandler = new RegisterUser(context, mockUserManager, mockJwtGenerator, mockEventBus);
                var registeredUser = await registerCommandHandler.Handle(registerCommand, CancellationToken.None);
                
                Assert.NotNull(registeredUser);
                Assert.Equal(registeredUser.Username, createdUser.UserName);
            }
        }
        
        [Fact]
        public async Task GivenRegisterCommandHandler_WhenNewUserHasRegistered_ThenUserRegisteredEventShouldBePublished()
        {
            var options = SqliteInMemory.CreateOptions<AuthenticationDbContext>();
            await using var context = new AuthenticationDbContext(options);
            {
                await context.Database.EnsureCreatedAsync();
                context.SeedUserData();

                var registerCommand = new PaperStreet.Authentication.Application.Commands.RegisterUser.Command
                {
                    DisplayName = "Test User",
                    Username = "testuser@gmail.com",
                    Email = "testuser@gmail.com",
                    Password = "password123"
                };

                var createdUser = _fixture.TestUser;
                var mockUserManager = _fixture.UserManager;    
                var mockJwtGenerator = _fixture.JwtGenerator;
                var mockEventBus = _fixture.EventBus;
                
                mockUserManager.CreateAsync(createdUser, registerCommand.Password)
                    .ReturnsForAnyArgs(Task.FromResult(IdentityResult.Success));

                var registerCommandHandler = new RegisterUser(context, mockUserManager, mockJwtGenerator, mockEventBus);
                await registerCommandHandler.Handle(registerCommand, CancellationToken.None);
                
                mockEventBus.Received().Publish(Arg.Any<UserRegisteredEvent>());
            }
        }
    }
}