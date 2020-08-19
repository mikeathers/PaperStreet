using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using PaperStreet.Authentication.Application.CommandHandlers;
using PaperStreet.Authentication.Data.Context;
using PaperStreet.Authentication.Domain.Models;
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

                var registerCommand = new PaperStreet.Authentication.Application.Commands.Register.Command
                {
                    DisplayName = "Test User",
                    Username = "test@gmail.com",
                    Email = "test@gmail.com",
                    Password = "password123"
                };

                var mockUserManager = _fixture.UserManager;
                var mockJwtGenerator = _fixture.JwtGenerator;
                
                var registerCommandHandler = new Register(context, mockUserManager, mockJwtGenerator);

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

                var registerCommand = new PaperStreet.Authentication.Application.Commands.Register.Command
                {
                    DisplayName = "Test User",
                    Username = "test@gmail.com",
                    Email = "test@gmail.com1",
                    Password = "password123"
                };

                var mockUserManager = _fixture.UserManager;
                var mockJwtGenerator = _fixture.JwtGenerator;
                
                var registerCommandHandler = new Register(context, mockUserManager, mockJwtGenerator);

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

                var registerCommand = new PaperStreet.Authentication.Application.Commands.Register.Command
                {
                    DisplayName = "Test User",
                    Username = "testuser@gmail.com",
                    Email = "testuser@gmail.com",
                    Password = "password123"
                };

                var createdUser = _fixture.TestUser;
                var mockUserManager = _fixture.UserManager;    
                var mockJwtGenerator = _fixture.JwtGenerator;
                
                mockUserManager.CreateAsync(createdUser, registerCommand.Password).ReturnsForAnyArgs(Task.FromResult(IdentityResult.Success));

                var registerCommandHandler = new Register(context, mockUserManager, mockJwtGenerator);
                var registeredUser = await registerCommandHandler.Handle(registerCommand, CancellationToken.None);
                
                Assert.NotNull(registeredUser);
                Assert.Equal(registeredUser.Username, createdUser.UserName);
            }
        }
    }
}