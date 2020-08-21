using NSubstitute;
using PaperStreet.Communication.Application.EventHandlers;
using PaperStreet.Communication.Application.Interfaces;
using PaperStreet.Domain.Core.Events.User.Communication;
using PaperStreet.Domain.Core.Models;
using Xunit;

namespace PaperStreet.Tests.Microservices.Communication.Application.EventHandlers
{
    public class SendEmailEventHandlerTests
    {
        [Fact]
        public void GivenSendEmailEventHandler_WhenEmailIsReceived_ThenShouldCallEmailSender()
        {
            var emailToSend = new Email();
            var sendEmailEvent = new SendEmailEvent(emailToSend);
            var mockEmailSender = Substitute.For<IEmailSender>();
            
            var sendEmailEventHandler = new SendEmailEventHandler(mockEmailSender);

            sendEmailEventHandler.Handle(sendEmailEvent);

            mockEmailSender.Received().SendEmail(emailToSend);
        }
    }
}