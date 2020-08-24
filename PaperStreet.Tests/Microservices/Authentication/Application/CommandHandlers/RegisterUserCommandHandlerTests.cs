using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NWebsec.Core.Common.HttpHeaders;
using PaperStreet.Authentication.Application.CommandHandlers;
using PaperStreet.Authentication.Application.Commands;
using PaperStreet.Authentication.Application.Interfaces;
using PaperStreet.Authentication.Data.Context;
using PaperStreet.Authentication.Domain.Models;
using PaperStreet.Domain.Core.Bus;
using PaperStreet.Domain.Core.Events.User.Logging;
using PaperStreet.Domain.Core.Models;
using PaperStreet.Tests.Microservices.Authentication.Fixture;
using PaperStreet.Tests.Microservices.Authentication.SeedData;
using TestSupport.EfHelpers;
using Xunit;

namespace PaperStreet.Tests.Microservices.Authentication.Application.CommandHandlers
{
    public class RegisterUserCommandHandlerTests : IClassFixture<AuthenticationFixture>
    {
        private readonly UserManager<AppUser> _mockUserManager;
        private readonly IJwtGenerator _mockJwtGenerator;
        private readonly IEventBus _mockEventBus;
        private readonly IUserConfirmationEmail _mockUserConfirmationEmail;
        private readonly IFailedIdentityResult _mockFailedIdentityResult;
        private readonly RegisterUser.Command _command;
        private readonly AppUser _user;
            
        public RegisterUserCommandHandlerTests(AuthenticationFixture fixture)
        {
            _mockUserConfirmationEmail = fixture.UserConfirmationEmail;
            _mockUserManager = fixture.UserManager;
            _mockJwtGenerator = fixture.JwtGenerator;
            _mockEventBus = fixture.EventBus;
            _mockFailedIdentityResult = fixture.FailedIdentityResult;
            _user = fixture.TestUser;
            
            _command = new RegisterUser.Command
            {
                FirstName = "Test User",
                Email = "testuser@gmail.com",
                Password = "password123"
            };
        }

        [Fact]
        public async Task
            GivenRegisterUserCommandHandler_WhenNewUserDetailsAreProvided_ThenShouldCallUserManagerToFindExistingUser()
        {
            _mockUserManager.FindByEmailAsync(_command.Email).ReturnsNullForAnyArgs();
            _mockUserManager.CreateAsync(_user, _command.Password)
                .ReturnsForAnyArgs(Task.FromResult(IdentityResult.Success));

            var registerCommandHandler = 
                new RegisterUserCommandHandler(_mockUserManager, _mockJwtGenerator, _mockEventBus,
                    _mockUserConfirmationEmail, _mockFailedIdentityResult);
            
            await registerCommandHandler.Handle(_command, CancellationToken.None);

            await _mockUserManager.Received().FindByEmailAsync(Arg.Any<string>());
        }
        
        [Fact]
        public async Task GivenRegisterUserCommandHandler_WhenUserEmailAlreadyExists_ThenThrowsRestException()
        {
            _mockUserManager.FindByEmailAsync(_command.Email).Returns(_user);

            var registerCommandHandler =
                new RegisterUserCommandHandler(_mockUserManager, _mockJwtGenerator, _mockEventBus,
                    _mockUserConfirmationEmail, _mockFailedIdentityResult);
        
            await Assert.ThrowsAsync<RestException>(() =>
                registerCommandHandler.Handle(_command, CancellationToken.None));
        }
        
        [Fact]
        public async Task
            GivenRegisterUserCommandHandler_WhenNewUserDetailsAreProvided_ThenShouldCallUserManagerToCreateUser()
        {
            _mockUserManager.FindByEmailAsync(_command.Email).ReturnsNullForAnyArgs();
            _mockUserManager.CreateAsync(Arg.Any<AppUser>(), Arg.Any<string>())
                .ReturnsForAnyArgs(Task.FromResult(IdentityResult.Success));
            
        
            var registerCommandHandler = 
                new RegisterUserCommandHandler(_mockUserManager, _mockJwtGenerator, _mockEventBus,
                    _mockUserConfirmationEmail, _mockFailedIdentityResult);
            
            await registerCommandHandler.Handle(_command, CancellationToken.None);
        
            await _mockUserManager.Received().CreateAsync(Arg.Any<AppUser>(), Arg.Any<string>());
        }
        
        [Fact]
        public async Task
            GivenRegisterUserCommandHandler_WhenNewUserFailedToRegister_ThenShouldCallFailedIdentityResult()
        {
            _mockUserManager.FindByEmailAsync(_command.Email).ReturnsNullForAnyArgs();
            _mockUserManager.CreateAsync(_user, _command.Password)
                .ReturnsForAnyArgs(Task.FromResult(IdentityResult.Failed()));
        
            var registerCommandHandler = 
                new RegisterUserCommandHandler(_mockUserManager, _mockJwtGenerator, _mockEventBus,
                    _mockUserConfirmationEmail, _mockFailedIdentityResult);
            
            await registerCommandHandler.Handle(_command, CancellationToken.None);
        
            _mockFailedIdentityResult.Received()
                .Handle(Arg.Any<AppUser>(), Arg.Any<List<IdentityError>>(), Arg.Any<string>());
        }
        
        [Fact]
        public async Task
            GivenRegisterUserCommandHandler_WhenNewUserHasRegistered_ThenShouldPublishAuthenticationLogEvent()
        {
            _mockUserManager.FindByEmailAsync(_command.Email).ReturnsNullForAnyArgs();
            _mockUserManager.CreateAsync(_user, _command.Password)
                .ReturnsForAnyArgs(Task.FromResult(IdentityResult.Success));
        
            var registerCommandHandler = 
                new RegisterUserCommandHandler(_mockUserManager, _mockJwtGenerator, _mockEventBus,
                    _mockUserConfirmationEmail, _mockFailedIdentityResult);
            
            await registerCommandHandler.Handle(_command, CancellationToken.None);
            
            _mockEventBus.Received().Publish(Arg.Any<AuthenticationLogEvent>());
        }
        
        [Fact]
        public async Task
            GivenRegisterUserCommandHandler_WhenNewUserHasRegistered_ThenUserConfirmationEmailShouldBeCalled()
        {
            _mockUserManager.FindByEmailAsync(_command.Email).ReturnsNullForAnyArgs();
            _mockUserManager.CreateAsync(_user, _command.Password)
                .ReturnsForAnyArgs(Task.FromResult(IdentityResult.Success));
        
            var registerCommandHandler =
                new RegisterUserCommandHandler(_mockUserManager, _mockJwtGenerator, _mockEventBus,
                    _mockUserConfirmationEmail, _mockFailedIdentityResult);
        
            await registerCommandHandler.Handle(_command, CancellationToken.None);
        
            await _mockUserConfirmationEmail.Received().Send(Arg.Any<AppUser>());
        }
        
        [Fact]
        public async Task GivenRegisterUserCommandHandler_WhenNewUserDetailsAreProvided_ThenCreatesNewUser()
        {
            _mockUserManager.FindByEmailAsync(_command.Email).ReturnsNullForAnyArgs();
            _mockUserManager.CreateAsync(_user, _command.Password)
                .ReturnsForAnyArgs(Task.FromResult(IdentityResult.Success));
        
            var registerCommandHandler = 
                new RegisterUserCommandHandler(_mockUserManager, _mockJwtGenerator, _mockEventBus,
                    _mockUserConfirmationEmail, _mockFailedIdentityResult);
            
            var registeredUser = await registerCommandHandler.Handle(_command, CancellationToken.None);
            
            Assert.NotNull(registeredUser);
        }
    }
}