using System.Net;
using System.Threading.Tasks;
using NSubstitute;
using PaperStreet.Communication.Application.Interfaces;
using PaperStreet.Communication.Application.Services;
using PaperStreet.Domain.Core.Bus;
using PaperStreet.Domain.Core.Events.Errors;
using PaperStreet.Domain.Core.Models;
using Xunit;

namespace PaperStreet.Tests.Microservices.Communication.Application.Services
{
    public class EmailSenderTests
    {
        [Fact]
        public async Task GivenEmailSender_WhenEmailFailsToSend_ThenShouldPublishEmailFailedToSendEvent()
        {
            const string to = "test@gmail.com";
            const string firstName = "Joe";
            const string subject = "Test Subject";
            const string htmlContent = "<h1>Test Html Content</h1>";
            const string plainTextContent = "Test Plain Text Content";

            var emailToSend = new Email
            {
                To = to,
                FirstName = firstName,
                Subject = subject,
                HtmlContent = htmlContent,
                PlainTextContent = plainTextContent
            };

            var mockSendGridClient = Substitute.For<ISendGridClient>();
            var mockEventBus = Substitute.For<IEventBus>();
            
            mockSendGridClient.SendEmailAsync(emailToSend)
                .ReturnsForAnyArgs(HttpStatusCode.NotFound);
            
            var emailSender = new EmailSender(mockSendGridClient, mockEventBus);
            await emailSender.SendEmail(emailToSend);
            
            mockEventBus.Received().Publish(Arg.Any<EmailFailedToSendEvent>());
        }
    }
}