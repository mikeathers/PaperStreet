using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using PaperStreet.Authentication.Application.Interfaces;
using PaperStreet.Authentication.Application.Queries;
using PaperStreet.Authentication.Application.QueryHandlers;
using PaperStreet.Domain.Core.Events.User;
using PaperStreet.Domain.Core.Models;
using PaperStreet.Tests.Microservices.Authentication.Fixture;
using Xunit;

namespace PaperStreet.Tests.Microservices.Authentication.Application.QueryHandlers
{
    public class LoginUserQueryHandlerTests : IClassFixture<AuthenticationFixture>
    {
        private readonly AuthenticationFixture _fixture;

        public LoginUserQueryHandlerTests(AuthenticationFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GivenLoginUserQueryHandler_WhenCorrectLoginInfoProvided_ThenShouldAuthenticateUser()
        {
            var loginQuery = new LoginUser.Query
            {
                Email = "testuser@gmail.com",
                Password = "Password123!"
            };

            var existingUser = _fixture.TestUser;
            var mockUserManager = _fixture.UserManager;
            var mockEventBus = _fixture.EventBus;
            var mockJwtGenerator = Substitute.For<IJwtGenerator>();

            mockUserManager.FindByEmailAsync(loginQuery.Email).ReturnsForAnyArgs(existingUser);
            mockUserManager.CheckPasswordAsync(existingUser, loginQuery.Password).ReturnsForAnyArgs(true);
            
            var loginUserQueryHandler = new LoginUserQueryHandler(mockUserManager, mockJwtGenerator, mockEventBus);

            var authenticatedUser = await loginUserQueryHandler.Handle(loginQuery, CancellationToken.None);
            
            Assert.NotNull(authenticatedUser);
            Assert.Equal(authenticatedUser.Email, loginQuery.Email);
        }
        
        [Fact]
        public async Task GivenLoginUserQueryHandler_WhenIncorrectLoginInfoProvided_ThenShouldRaiseException()
        {
            var loginQuery = new LoginUser.Query
            {
                Email = "testuser@gmail.com",
                Password = "Password123!"
            };

            var mockUserManager = _fixture.UserManager;
            var mockEventBus = _fixture.EventBus;
            var mockJwtGenerator = Substitute.For<IJwtGenerator>();

            mockUserManager.FindByEmailAsync(loginQuery.Email).ReturnsNullForAnyArgs();
            
            var loginUserQueryHandler = new LoginUserQueryHandler(mockUserManager, mockJwtGenerator, mockEventBus);
            
            await Assert.ThrowsAsync<RestException>(() => loginUserQueryHandler.Handle(loginQuery, CancellationToken.None));
        }
        
        [Fact]
        public async Task GivenLoginUserQueryHandler_WhenUserAuthenticated_ThenShouldPublishUserLoginEvent()
        {
            var loginQuery = new LoginUser.Query
            {
                Email = "testuser@gmail.com",
                Password = "Password123!"
            };

            var existingUser = _fixture.TestUser;
            var mockUserManager = _fixture.UserManager;
            var mockEventBus = _fixture.EventBus;
            var mockJwtGenerator = Substitute.For<IJwtGenerator>();

            mockUserManager.FindByEmailAsync(loginQuery.Email).ReturnsForAnyArgs(existingUser);
            mockUserManager.CheckPasswordAsync(existingUser, loginQuery.Password).ReturnsForAnyArgs(true);
            
            var loginUserQueryHandler = new LoginUserQueryHandler(mockUserManager, mockJwtGenerator, mockEventBus);

            await loginUserQueryHandler.Handle(loginQuery, CancellationToken.None);
            
            mockEventBus.Received().Publish(Arg.Any<UserLoginEvent>());
        }
    }
}