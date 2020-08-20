using System.Net;
using System.Threading.Tasks;
using PaperStreet.Communication.Application.Interfaces;
using PaperStreet.Domain.Core.Bus;
using PaperStreet.Domain.Core.Events.Errors;
using PaperStreet.Domain.Core.KeyValuePairs;
using PaperStreet.Domain.Core.Models;

namespace PaperStreet.Communication.Application.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly ISendGridClient _sendGridClient;
        private readonly IEventBus _eventBus;

        public EmailSender(ISendGridClient sendGridClient, IEventBus eventBus)
        {
            _sendGridClient = sendGridClient;
            _eventBus = eventBus;
        }

        public async Task SendEmail(Email emailToSend)
        {
            var emailSentStatusCode = await _sendGridClient.SendEmailAsync(emailToSend);
            if (emailSentStatusCode != HttpStatusCode.OK
                || emailSentStatusCode != HttpStatusCode.Accepted)
            {
                _eventBus.Publish(new EmailFailedToSendEvent(emailToSend.UserId, ErrorMessages.EmailFailedToSend));
            }
        }
    }
}