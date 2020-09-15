using System.Threading.Tasks;
using FluentValidation;
using PaperStreet.Authentication.Application.Validators;
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
        [Theory]
        [InlineData("", false)]
        [InlineData("Pa123", false)]
        [InlineData("password123!", false)]
        [InlineData("PASSWORD123!", false)]
        [InlineData("Password!", false)]
        [InlineData("Password123", false)]
        [InlineData("Password123!", true)]
        public async Task GivenAPassword_WhenValidated_ThenShouldReturnExpectedResult(string passwordToTest,
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