using System;
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
using PaperStreet.Domain.Core.Events.User.Logging;
using PaperStreet.Domain.Core.Models;
using PaperStreet.Tests.Microservices.Authentication.Fixture;
using Xunit;

namespace PaperStreet.Tests.Microservices.Authentication.Application.QueryHandlers
{
    public class RefreshTokenQueryHandlerTests : IClassFixture<AuthenticationFixture>
    {
        private readonly UserManager<AppUser> _mockUserManager;
        private readonly IJwtGenerator _mockJwtGenerator;
        private readonly IEventBus _mockEventBus;
        private readonly RefreshTokenQuery _query;
        private readonly AppUser _user;
        private readonly AuthenticationFixture _fixture;
        
        public RefreshTokenQueryHandlerTests(AuthenticationFixture fixture)
        {
            _mockUserManager = fixture.UserManager;
            _mockJwtGenerator = fixture.JwtGenerator;
            _mockEventBus = fixture.EventBus;
            _user = fixture.TestUser;
            _fixture = fixture;
            
            _query = new RefreshTokenQuery
            {    
                Token = "1010010",
                RefreshToken = fixture.UserRefreshToken
            };

        }

        [Fact]
        public async Task GivenRefreshTokenQueryHandler_WhenCorrectQueryReceived_ThenShouldCallUserManagerToFindUser()
        {
            _user.RefreshToken = _fixture.UserRefreshToken;
            _mockUserManager.FindByEmailAsync(_query.Email).ReturnsForAnyArgs(_user);

            var refreshTokenQueryHandler =
                new RefreshTokenQueryHandler(_mockUserManager, _mockJwtGenerator, _mockEventBus);
        
            await refreshTokenQueryHandler.Handle(_query, CancellationToken.None);
        
            await _mockUserManager.Received().FindByEmailAsync(_query.Email);
        }

        [Fact]
        public async Task GivenRefreshTokenQueryHandler_WhenUserIsNotFound_ThenShouldThrowRestException()
        {
            _mockUserManager.FindByEmailAsync(_query.Email).ReturnsNullForAnyArgs();
            
            var refreshTokenQueryHandler =
                new RefreshTokenQueryHandler(_mockUserManager, _mockJwtGenerator, _mockEventBus);

            await Assert.ThrowsAsync<RestException>(() =>
                refreshTokenQueryHandler.Handle(_query, CancellationToken.None));
        }
        
        [Fact]
        public async Task
            GivenRefreshTokenQueryHandler_WhenRefreshTokenDoesNotMatchStoredRefreshToken_ThenShouldThrowRestException()
        {
            _query.RefreshToken = "10101010";
            
            _mockUserManager.FindByEmailAsync(_query.Email).ReturnsForAnyArgs(_user);
            
            var refreshTokenQueryHandler =
                new RefreshTokenQueryHandler(_mockUserManager, _mockJwtGenerator, _mockEventBus);

            await Assert.ThrowsAsync<RestException>(() =>
                refreshTokenQueryHandler.Handle(_query, CancellationToken.None));
        }
        
        [Fact]
        public async Task
            GivenRefreshTokenQueryHandler_WhenRefreshTokenHasExpired_ThenShouldThrowRestException()
        {
            _user.RefreshTokenExpiry = DateTime.Now.AddHours(-1);
            
            _mockUserManager.FindByEmailAsync(_query.Email).ReturnsForAnyArgs(_user);
            
            var refreshTokenQueryHandler =
                new RefreshTokenQueryHandler(_mockUserManager, _mockJwtGenerator, _mockEventBus);

            await Assert.ThrowsAsync<RestException>(() =>
                refreshTokenQueryHandler.Handle(_query, CancellationToken.None));
        }
        
        [Fact]
        public async Task
            GivenRefreshTokenQueryHandler_WhenCorrectQueryReceived_ThenShouldCallJwtGeneratorToCreateRefreshToken()
        {
            _user.RefreshToken = _fixture.UserRefreshToken;
            _mockUserManager.FindByEmailAsync(_query.Email).ReturnsForAnyArgs(_user);
            
            var refreshTokenQueryHandler =
                new RefreshTokenQueryHandler(_mockUserManager, _mockJwtGenerator, _mockEventBus);
        
            await refreshTokenQueryHandler.Handle(_query, CancellationToken.None);
        
            _mockJwtGenerator.Received().GenerateRefreshToken();
        }
        
        [Fact]
        public async Task GivenRefreshTokenQueryHandler_WhenCorrectQueryReceived_ThenShouldCallUserManagerToUpdateUser()
        {
            _mockUserManager.FindByEmailAsync(_query.Email).ReturnsForAnyArgs(_user);
            
            var refreshTokenQueryHandler =
                new RefreshTokenQueryHandler(_mockUserManager, _mockJwtGenerator, _mockEventBus);

            await refreshTokenQueryHandler.Handle(_query, CancellationToken.None);

            await _mockUserManager.Received().UpdateAsync(Arg.Any<AppUser>());
        }
        
        [Fact]
        public async Task GivenRefreshTokenQueryHandler_WhenCorrectQueryReceived_ThenShouldCallEventBus()
        {
            _user.RefreshToken = _fixture.UserRefreshToken;
            _mockUserManager.FindByEmailAsync(_query.Email).ReturnsForAnyArgs(_user);
            
            var refreshTokenQueryHandler =
                new RefreshTokenQueryHandler(_mockUserManager, _mockJwtGenerator, _mockEventBus);

            await refreshTokenQueryHandler.Handle(_query, CancellationToken.None);

            _mockEventBus.Received().Publish(Arg.Any<UpdatedRefreshTokenEvent>());
        }
        
        [Fact]
        public async Task
            GivenRefreshTokenQueryHandler_WhenCorrectQueryReceived_ThenShouldCallJwtGeneratorToCreateToken()
        {
            _user.RefreshToken = _fixture.UserRefreshToken;
            _user.RefreshTokenExpiry = DateTime.Now.AddHours(1);
            _mockUserManager.FindByEmailAsync(_query.Email).ReturnsForAnyArgs(_user);
            
            var refreshTokenQueryHandler =
                new RefreshTokenQueryHandler(_mockUserManager, _mockJwtGenerator, _mockEventBus);
        
            await refreshTokenQueryHandler.Handle(_query, CancellationToken.None);
        
            _mockJwtGenerator.Received().CreateToken(Arg.Any<AppUser>());
        }
        
        [Fact]
        public async Task GivenRefreshTokenQueryHandler_WhenCorrectQueryReceived_ThenShouldReturnUser()
        {
            _user.RefreshToken = _fixture.UserRefreshToken;
            _mockUserManager.FindByEmailAsync(_query.Email).ReturnsForAnyArgs(_user);
            
            var refreshTokenQueryHandler =
                new RefreshTokenQueryHandler(_mockUserManager, _mockJwtGenerator, _mockEventBus);
        
            var updatedUser = await refreshTokenQueryHandler.Handle(_query, CancellationToken.None);
        
            Assert.NotNull(updatedUser);
        }
    }
}