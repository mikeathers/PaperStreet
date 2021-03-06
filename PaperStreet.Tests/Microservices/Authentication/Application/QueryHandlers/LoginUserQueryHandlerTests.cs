using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using PaperStreet.Authentication.Application.Interfaces;
using PaperStreet.Authentication.Application.Queries;
using PaperStreet.Authentication.Application.QueryHandlers;
using PaperStreet.Authentication.Domain.Models;
using PaperStreet.Domain.Core.Bus;
using PaperStreet.Domain.Core.Events.Errors;
using PaperStreet.Domain.Core.Events.User.Logging;
using PaperStreet.Domain.Core.Models;
using PaperStreet.Tests.Microservices.Authentication.Fixture;
using Xunit;

namespace PaperStreet.Tests.Microservices.Authentication.Application.QueryHandlers
{
    public class LoginUserQueryHandlerTests : IClassFixture<AuthenticationFixture>
    {
        private readonly UserManager<AppUser> _mockUserManager;
        private readonly IJwtGenerator _mockJwtGenerator;
        private readonly IEventBus _mockEventBus;
        private readonly LoginUserQuery _query;
        private readonly AppUser _user;

        public LoginUserQueryHandlerTests(AuthenticationFixture fixture)
        {
            _mockUserManager = fixture.UserManager;
            _mockJwtGenerator = fixture.JwtGenerator;
            _mockEventBus = fixture.EventBus;
            _user = fixture.TestUser;
            
            _query = new LoginUserQuery
            {
                Email = "testuser@gmail.com",
                Password = "Password123!"
            };
        }

        [Fact]
        public async Task GivenLoginUserQueryHandler_WhenCorrectLoginInfoProvided_ThenShouldCallUserManagerToFindUser()
        {
            _mockUserManager.FindByEmailAsync(_query.Email).ReturnsForAnyArgs(_user);
            _mockUserManager.CheckPasswordAsync(_user, _query.Password).ReturnsForAnyArgs(true);
            
            var loginUserQueryHandler = new LoginUserQueryHandler(_mockUserManager, _mockJwtGenerator, _mockEventBus);

            await loginUserQueryHandler.Handle(_query, CancellationToken.None);
            
            await _mockUserManager.Received().FindByEmailAsync(Arg.Any<string>());
        }
        
        [Fact]
        public async Task GivenLoginUserQueryHandler_WhenIncorrectLoginInfoProvided_ThenShouldRaiseException()
        {
            _mockUserManager.FindByEmailAsync(_query.Email).ReturnsNullForAnyArgs();
            
            var loginUserQueryHandler = new LoginUserQueryHandler(_mockUserManager, _mockJwtGenerator, _mockEventBus);
            
            await Assert.ThrowsAsync<RestException>(() => loginUserQueryHandler.Handle(_query, CancellationToken.None));
        }
        
        [Fact]
        public async Task
            GivenLoginUserQueryHandler_WhenCorrectLoginInfoProvided_ThenShouldCallUserManagerToCheckPassword()
        {
            _mockUserManager.FindByEmailAsync(_query.Email).ReturnsForAnyArgs(_user);
            _mockUserManager.CheckPasswordAsync(_user, _query.Password).ReturnsForAnyArgs(true);
            
            var loginUserQueryHandler = new LoginUserQueryHandler(_mockUserManager, _mockJwtGenerator, _mockEventBus);

            await loginUserQueryHandler.Handle(_query, CancellationToken.None);
            
            await _mockUserManager.Received().CheckPasswordAsync(Arg.Any<AppUser>(), Arg.Any<string>());
        }
        
        [Fact]
        public async Task
            GivenLoginUserQueryHandler_WhenCheckingPasswordFails_ThenShouldPublishLogErrorEvent()
        {
            _mockUserManager.FindByEmailAsync(_query.Email).ReturnsForAnyArgs(_user);
            _mockUserManager.CheckPasswordAsync(_user, _query.Password).ReturnsForAnyArgs(false);
            
            var loginUserQueryHandler = new LoginUserQueryHandler(_mockUserManager, _mockJwtGenerator, _mockEventBus);

            try
            {
                await loginUserQueryHandler.Handle(_query, CancellationToken.None);
            }
            catch
            {
                _mockEventBus.Received().Publish(Arg.Any<ErrorLogEvent>());
            }
        }
        
        [Fact]
        public async Task GivenLoginUserQueryHandler_WhenCheckingPasswordFails_ThenShouldThrowRestException()
        {
            _mockUserManager.FindByEmailAsync(_query.Email).ReturnsForAnyArgs(_user);
            _mockUserManager.CheckPasswordAsync(_user, _query.Password).ReturnsForAnyArgs(false);
            
            var loginUserQueryHandler = new LoginUserQueryHandler(_mockUserManager, _mockJwtGenerator, _mockEventBus);

            await Assert.ThrowsAsync<RestException>(() =>loginUserQueryHandler.Handle(_query, CancellationToken.None));
        }
        
        [Fact]
        public async Task GivenLoginUserQueryHandler_WhenUserAuthenticated_ThenShouldPublishAuthenticationLogEvent()
        {
            _mockUserManager.FindByEmailAsync(_query.Email).ReturnsForAnyArgs(_user);
            _mockUserManager.CheckPasswordAsync(_user, _query.Password).ReturnsForAnyArgs(true);
            
            var loginUserQueryHandler = new LoginUserQueryHandler(_mockUserManager, _mockJwtGenerator, _mockEventBus);

            await loginUserQueryHandler.Handle(_query, CancellationToken.None);
            
            _mockEventBus.Received().Publish(Arg.Any<AuthenticationLogEvent>());
        }
        
        [Fact]
        public async Task GivenLoginUserQueryHandler_WhenCorrectLoginInfoProvided_ThenShouldAuthenticateUser()
        {
            _mockUserManager.FindByEmailAsync(_query.Email).ReturnsForAnyArgs(_user);
            _mockUserManager.CheckPasswordAsync(_user, _query.Password).ReturnsForAnyArgs(true);
            
            var loginUserQueryHandler = new LoginUserQueryHandler(_mockUserManager, _mockJwtGenerator, _mockEventBus);

            var authenticatedUser = await loginUserQueryHandler.Handle(_query, CancellationToken.None);
            
            Assert.NotNull(authenticatedUser);
            Assert.Equal(authenticatedUser.Email, _query.Email);
        }
    }
}