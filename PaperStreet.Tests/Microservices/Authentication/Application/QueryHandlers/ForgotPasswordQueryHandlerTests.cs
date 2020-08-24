using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using PaperStreet.Authentication.Application.CommandHandlers;
using PaperStreet.Authentication.Application.Commands;
using PaperStreet.Authentication.Application.Interfaces;
using PaperStreet.Authentication.Application.Queries;
using PaperStreet.Authentication.Application.QueryHandlers;
using PaperStreet.Authentication.Domain.Models;
using PaperStreet.Domain.Core.Bus;
using PaperStreet.Domain.Core.Events.User.Communication;
using PaperStreet.Domain.Core.Events.User.Logging;
using PaperStreet.Domain.Core.Models;
using PaperStreet.Tests.Microservices.Authentication.Fixture;
using Xunit;

namespace PaperStreet.Tests.Microservices.Authentication.Application.QueryHandlers
{
    public class ForgotPasswordCommandHandlerTests : IClassFixture<AuthenticationFixture>
    {
        private readonly UserManager<AppUser> _mockUserManager;
        private readonly IEmailBuilder _mockEmailBuilder;
        private readonly IEventBus _mockEventBus;
        private readonly AppUser _user;
        private readonly ForgotPassword.Query _query;
        
        public ForgotPasswordCommandHandlerTests(AuthenticationFixture fixture)
        {
            _mockUserManager = fixture.UserManager;
            _mockEmailBuilder = fixture.EmailBuilder;
            _mockEventBus = fixture.EventBus;
            _user = fixture.TestUser;

            _query = new ForgotPassword.Query
            {
                Email = "test@gmail.com"
            };
        }
        
        [Fact]
        public async Task GivenForgotCommandHandler_WhenCorrectQueryReceived_ThenShouldUseUserManagerToFindUser()
        {
            _mockUserManager.FindByEmailAsync(_query.Email).ReturnsForAnyArgs(_user);
            
            var forgotPasswordQueryHandler = new ForgotPasswordCommandHandler(_mockUserManager, _mockEmailBuilder, _mockEventBus);
            
            await forgotPasswordQueryHandler.Handle(_query, CancellationToken.None);

            await _mockUserManager.ReceivedWithAnyArgs().FindByEmailAsync(_query.Email);
        }
        
        [Fact]
        public async Task GivenForgotPasswordQueryHandler_WhenUserNotFound_ThenShouldThrowRestException()
        {
            _mockUserManager.FindByEmailAsync(_query.Email).ReturnsNullForAnyArgs();
            
            var forgotPasswordQueryHandler = new ForgotPasswordCommandHandler(_mockUserManager, _mockEmailBuilder, _mockEventBus);

            await Assert.ThrowsAsync<RestException>(() =>
                forgotPasswordQueryHandler.Handle(_query, CancellationToken.None));
        }

        [Fact]
        public async Task GivenForgotPasswordQueryHandler_WhenReceivesCorrectCommand_ThenShouldCallUserManagerToGeneratePasswordResetToken()
        {
            _mockUserManager.FindByEmailAsync(_query.Email).ReturnsForAnyArgs(_user);
            
            var forgotPasswordQueryHandler = new ForgotPasswordCommandHandler(_mockUserManager, _mockEmailBuilder, _mockEventBus);
            
            await forgotPasswordQueryHandler.Handle(_query, CancellationToken.None);

            await _mockUserManager.Received().GeneratePasswordResetTokenAsync(Arg.Any<AppUser>());
        }
        
        [Fact]
        public async Task GivenForgotPasswordQueryHandler_WhenReceivesCorrectCommand_ThenShouldCallEmailBuilderToGetPasswordResetEmail()
        {
            _mockUserManager.FindByEmailAsync(_query.Email).ReturnsForAnyArgs(_user);
            
            var forgotPasswordQueryHandler = new ForgotPasswordCommandHandler(_mockUserManager, _mockEmailBuilder, _mockEventBus);
            
            await forgotPasswordQueryHandler.Handle(_query, CancellationToken.None);

            _mockEmailBuilder.Received().ResetPasswordEmail(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
        }
        
        [Fact]
        public async Task GivenForgotPasswordQueryHandler_WhenReceivesCorrectCommand_ThenShouldPublishSendEmailEvent()
        {
            _mockUserManager.FindByEmailAsync(_query.Email).ReturnsForAnyArgs(_user);
            
            var forgotPasswordQueryHandler = new ForgotPasswordCommandHandler(_mockUserManager, _mockEmailBuilder, _mockEventBus);
            
            await forgotPasswordQueryHandler.Handle(_query, CancellationToken.None);

            _mockEventBus.Received().Publish(Arg.Any<SendEmailEvent>());
        }
        
        [Fact]
        public async Task GivenForgotPasswordQueryHandler_WhenReceivesCorrectCommand_ThenShouldPublishResetPasswordRequestEvent()
        {
            _mockUserManager.FindByEmailAsync(_query.Email).ReturnsForAnyArgs(_user);
            
            var forgotPasswordQueryHandler = new ForgotPasswordCommandHandler(_mockUserManager, _mockEmailBuilder, _mockEventBus);
            
            await forgotPasswordQueryHandler.Handle(_query, CancellationToken.None);

            _mockEventBus.Received().Publish(Arg.Any<ResetPasswordRequestEvent>());
        }
    }
}