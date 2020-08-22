using Microsoft.AspNetCore.Identity;
using NSubstitute;
using PaperStreet.Authentication.Application.Interfaces;
using PaperStreet.Authentication.Application.Services;
using PaperStreet.Authentication.Domain.Models;
using PaperStreet.Domain.Core.Bus;
using PaperStreet.Domain.Core.Events.User.Communication;
using PaperStreet.Domain.Core.Events.User.Logging;
using PaperStreet.Tests.Microservices.Authentication.Fixture;
using Xunit;

namespace PaperStreet.Tests.Microservices.Authentication.Application.Services
{
    public class UserConfirmationEmailTests : IClassFixture<AuthenticationFixture>
    {
        private readonly AppUser _user;
        private readonly IEmailBuilder _mockEmailBuilder;
        private readonly IEventBus _mockEventBus;
        private readonly UserManager<AppUser> _mockUserManager;
        
        public UserConfirmationEmailTests(AuthenticationFixture fixture)
        {
            _user = fixture.TestUser;
            _mockEmailBuilder = fixture.EmailBuilder;
            _mockEventBus = fixture.EventBus;
            _mockUserManager = fixture.UserManager;
        }
        
        [Fact]
        public void GivenUserConfirmationEmail_WhenReceivesCorrectData_ThenShouldCallUserManagerToGenerateToken()
        {
            var userConfirmationEmail = new UserConfirmationEmail(_mockUserManager, _mockEmailBuilder, _mockEventBus);
            userConfirmationEmail.Send(_user);
            
            _mockUserManager.Received().GenerateEmailConfirmationTokenAsync(_user);
        }
        
        [Fact]
        public void GivenUserConfirmationEmail_WhenReceivesCorrectData_ThenShouldCallEmailBuilderService()
        {
            var userConfirmationEmail = new UserConfirmationEmail(_mockUserManager, _mockEmailBuilder, _mockEventBus);
            userConfirmationEmail.Send(_user);
            
            _mockEmailBuilder.Received().ConfirmationEmail(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
        }
        
        [Fact]
        public void GivenUserConfirmationEmail_WhenReceivesCorrectData_ThenShouldPublishAuthenticationLogEvent()
        {
            var userConfirmationEmail = new UserConfirmationEmail(_mockUserManager, _mockEmailBuilder, _mockEventBus);
            userConfirmationEmail.Send(_user);
            
            _mockEventBus.Received().Publish(Arg.Any<AuthenticationLogEvent>());
        }
        
        [Fact]
        public void GivenUserConfirmationEmail_WhenReceivesCorrectData_ThenShouldPublishSendEmailEvent()
        {
            var userConfirmationEmail = new UserConfirmationEmail(_mockUserManager, _mockEmailBuilder, _mockEventBus);
            userConfirmationEmail.Send(_user);
            
            _mockEventBus.Received().Publish(Arg.Any<SendEmailEvent>());
        }
    }
}