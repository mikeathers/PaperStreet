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
        private readonly Email _emailToSend;
        private readonly ISendGridClient _mockSendGridClient;
        private readonly IEventBus _mockEventBus;
        
        public EmailSenderTests()
        {
            _mockSendGridClient = Substitute.For<ISendGridClient>();
            _mockEventBus = Substitute.For<IEventBus>();
            
            const string to = "test@gmail.com";
            const string firstName = "Joe";
            const string subject = "Test Subject";
            const string htmlContent = "<h1>Test Html Content</h1>";
            const string plainTextContent = "Test Plain Text Content";

            _emailToSend = new Email
            {
                To = to,
                FirstName = firstName,
                Subject = subject,
                HtmlContent = htmlContent,
                PlainTextContent = plainTextContent
            };
        }

        [Fact]
        public async Task GivenEmailSender_WhenEmailFailsToSend_ThenShouldPublishEmailFailedToSendEvent()
        {
            _mockSendGridClient.SendEmailAsync(_emailToSend)
                .ReturnsForAnyArgs(HttpStatusCode.NotFound);
            
            var emailSender = new EmailSender(_mockSendGridClient, _mockEventBus);
            await emailSender.SendEmail(_emailToSend);
            
            _mockEventBus.Received().Publish(Arg.Any<EmailFailedToSendEvent>());
        }
    }
}