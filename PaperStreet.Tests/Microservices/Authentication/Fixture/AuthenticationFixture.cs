using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using PaperStreet.Authentication.Application.Interfaces;
using PaperStreet.Authentication.Domain.Models;
using PaperStreet.Domain.Core.Bus;
using Xunit;

namespace PaperStreet.Tests.Microservices.Authentication.Fixture
{
    public class AuthenticationFixture
    {
        public AppUser TestUser { get; private set; }
        public IUserStore<AppUser> UserStore { get; private set;}
        public UserManager<AppUser> UserManager { get; private set; }
        
        public SignInManager<AppUser> SignInManager { get; private set; }
        public IJwtGenerator JwtGenerator { get; private set; }
        
        public IEventBus EventBus { get; private set; }
        
        public AuthenticationFixture()
        {
            CreateFixtureData();
        }

        private void CreateFixtureData()
        {
            var createdUser = Substitute.For<AppUser>();
            createdUser.Id = "0001";
            createdUser.UserName = "testuser@gmail.com";
            createdUser.Email = "testuser@gmail.com";
            createdUser.DisplayName = "Test User";
            TestUser = createdUser;

            var store = Substitute.For<IUserStore<AppUser>>();
            UserStore = store;
            
            var userManager =
                Substitute.For<UserManager<AppUser>>(store, null, null, null, null, null, null, null, null);
            UserManager = userManager;

            // var contextAccessor = Substitute.For<IHttpContextAccessor>();
            // var userPrincipalFactory = Substitute.For<IUserClaimsPrincipalFactory<AppUser>>();
            // var signInManager = Substitute.For<SignInManager<AppUser>>(userManager);
            // SignInManager = signInManager;

            var jwtGenerator = Substitute.For<IJwtGenerator>();
            JwtGenerator = jwtGenerator;

            var eventBus = Substitute.For<IEventBus>();
            EventBus = eventBus;

        }
        
    }
}