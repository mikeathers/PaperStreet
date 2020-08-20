using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using PaperStreet.Authentication.Application.Services;
using Xunit;

namespace PaperStreet.Tests.Microservices.Authentication.Application.Services
{
    public class EmailBuilderTests
    {
        [Fact]
        public void GivenEmailBuilder_WhenConfirmationEmailMethodFired_ThenShouldReturnStringOfHTML()
        {
            const string firstName = "Test User";
            const string userId = "1010101";
            const string emailConfirmationCode = "109d0d20ed0o-e2d";

            var mockConfiguration = Substitute.For<IConfiguration>();
            mockConfiguration.GetValue<string>("WebSiteUrl").Returns("localhost:5001");
            
            var emailBuilder = new EmailBuilder(mockConfiguration);
            var confirmationEmailHtml = emailBuilder.ConfirmationEmail(firstName, userId, emailConfirmationCode);

            Assert.IsType<string>(confirmationEmailHtml);
            Assert.Contains(firstName, confirmationEmailHtml);
            Assert.Contains(userId, confirmationEmailHtml);
            Assert.Contains(emailConfirmationCode, confirmationEmailHtml);
        }
    }
}