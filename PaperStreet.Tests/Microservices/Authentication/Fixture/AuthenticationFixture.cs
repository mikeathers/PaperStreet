using System;
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
        public UserManager<AppUser> UserManager { get; private set; }
        public IJwtGenerator JwtGenerator { get; private set; }
        public IEventBus EventBus { get; private set; }
        public IEmailBuilder EmailBuilder { get; private set; }
        public IUserConfirmationEmail UserConfirmationEmail { get; private set; }
        public IFailedIdentityResult FailedIdentityResult { get; private set; }
        public  string UserRefreshToken { get; set; }
        public DateTime UserRefreshTokenExpiry { get; set; }


        public AuthenticationFixture()
        {
            CreateFixtureData();
        }

        private void CreateFixtureData()
        {
            const string userRefreshToken = "20202020";
            var userRefreshTokenExpiry = DateTime.Now.AddHours(1);
            
            var createdUser = Substitute.For<AppUser>();
            createdUser.Id = "0001";
            createdUser.UserName = "testuser@gmail.com";
            createdUser.Email = "testuser@gmail.com";
            createdUser.FirstName = "Test User";
            createdUser.RefreshToken = userRefreshToken;
            createdUser.RefreshTokenExpiry = userRefreshTokenExpiry;

            UserRefreshToken = userRefreshToken;
            UserRefreshTokenExpiry = userRefreshTokenExpiry;

            TestUser = createdUser;

            var store = Substitute.For<IUserStore<AppUser>>();
            
            var userManager =
                Substitute.For<UserManager<AppUser>>(store, null, null, null, null, null, null, null, null);
            UserManager = userManager;

            var jwtGenerator = Substitute.For<IJwtGenerator>();
            JwtGenerator = jwtGenerator;

            var eventBus = Substitute.For<IEventBus>();
            EventBus = eventBus;

            var emailBuilder = Substitute.For<IEmailBuilder>();
            EmailBuilder = emailBuilder;

            var userConfirmationEmail = Substitute.For<IUserConfirmationEmail>();
            UserConfirmationEmail = userConfirmationEmail;

            var failedIdentityResult = Substitute.For<IFailedIdentityResult>();
            FailedIdentityResult = failedIdentityResult;
        }
    }
}