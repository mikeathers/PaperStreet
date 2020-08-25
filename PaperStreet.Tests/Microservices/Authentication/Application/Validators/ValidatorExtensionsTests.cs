using System.Threading.Tasks;
using FluentValidation;
using PaperStreet.Authentication.Domain.Validators;
using Xunit;

namespace PaperStreet.Tests.Microservices.Authentication.Application.Validators
{
    public class TestValidator : AbstractValidator<TestValidations>
    {
        public TestValidator()
        {
            RuleFor(x => x.Password).Password();
        }
    }
    
    public class TestValidations
    {
        public string Password { get; set; }
    }
    
    public class ValidatorExtensionsTests
    {
        [Fact]
        public async Task GivenACorrectPassword_WhenValidated_ThenShouldBeValid()
        {
            const string passwordToTest = "Password123!";
            
            var sut = new TestValidations
            {
                Password = passwordToTest
            };
                
            var validator = new TestValidator();
            var result = await validator.ValidateAsync(sut);
            Assert.True(result.IsValid);
        }
        
        [Theory]
        [InlineData("", false)]
        [InlineData("Pa123", false)]
        [InlineData("password123!", false)]
        [InlineData("PASSWORD123!", false)]
        [InlineData("Password!", false)]
        [InlineData("Password123", false)]
        public async Task GivenAnInvalidPassword_WhenValidated_ThenShouldBeInvalid(string passwordToTest,
            bool expectedResult)
        {
            var sut = new TestValidations
            {
                Password = passwordToTest
            };
                
            var validator = new TestValidator();
            var result = await validator.ValidateAsync(sut);
            Assert.Equal(result.IsValid, expectedResult);
        }
    }
}