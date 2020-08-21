using System.Threading.Tasks;
using NSubstitute;
using PaperStreet.Communication.Api.Controllers;
using PaperStreet.Communication.Application.Interfaces;
using PaperStreet.Domain.Core.Models;
using Xunit;

namespace PaperStreet.Tests.Microservices.Communication.Api.Controllers
{
    public class CommunicationControllerTests
    {
        [Fact]
        public void GivenSendEmailPostMethod_WhenReceivesCorrectEmailData_ThenShouldCallEmailSenderService()
        {
            var emailToSend = new Email();
            var mockEmailSenderService = Substitute.For<IEmailSender>();
            var communicationsController = new CommunicationController();

            communicationsController.SendEmail(emailToSend, mockEmailSenderService);

            mockEmailSenderService.Received().SendEmail(emailToSend);
        }
    }
}