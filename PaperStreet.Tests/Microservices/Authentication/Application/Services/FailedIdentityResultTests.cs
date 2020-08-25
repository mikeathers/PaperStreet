using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using PaperStreet.Authentication.Application.Services;
using PaperStreet.Authentication.Domain.Models;
using PaperStreet.Domain.Core.Bus;
using PaperStreet.Domain.Core.Events.Errors;
using PaperStreet.Tests.Microservices.Authentication.Fixture;
using Xunit;

namespace PaperStreet.Tests.Microservices.Authentication.Application.Services
{
    public class FailedIdentityResultTests : IClassFixture<AuthenticationFixture>
    {
        private readonly AppUser _user;
        private readonly List<IdentityError> _errors;
        
        public FailedIdentityResultTests(AuthenticationFixture fixture)
        {
            _user = fixture.TestUser;

            _errors = new List<IdentityError>
            {
                new IdentityError
                {
                    Code = "1",
                    Description = "First Error"
                },
                new IdentityError
                {
                    Code = "2",
                    Description = "Second Error"
                }
            }; 
        }
        
        [Fact]
        public void GivenFailedIdentityResult_WhenHandleIsCalledWithErrors_ThenShouldThrowException()
        {
            const string exception = "Problem in code";
            var mockEventBus = Substitute.For<IEventBus>();
            var failedIdentityResult = new FailedIdentityResult(mockEventBus);
        
            Assert.Throws<Exception>(() => failedIdentityResult.Handle(_user, _errors, exception));
        }
        
        [Fact]
        public void GivenFailedIdentityResult_WhenHandleIsCalledWith2Errors_ThenShouldPublish2LogErrorEvents()
        {
            const string exception = "Problem in code";
            
            var mockEventBus = Substitute.For<IEventBus>();
            var failedIdentityResult = new FailedIdentityResult(mockEventBus);

            try
            {
                failedIdentityResult.Handle(_user, _errors, exception);
            }
            catch 
            {
                mockEventBus.Received(2).Publish(Arg.Any<ErrorLogEvent>());    
            }
        }
    }
}