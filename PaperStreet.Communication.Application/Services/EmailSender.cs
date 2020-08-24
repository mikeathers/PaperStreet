using System.Data;
using System.Net;
using System.Threading.Tasks;
using FluentValidation;
using PaperStreet.Communication.Application.Interfaces;
using PaperStreet.Communication.Domain.Models;
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
            if (emailSentStatusCode != HttpStatusCode.Accepted)
            {
                _eventBus.Publish(new LogErrorEvent(emailToSend.UserId, ErrorMessages.EmailFailedToSend));
            }
        }
    }

    public class EmailSenderValidator : AbstractValidator<Email>
    {
        public EmailSenderValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.To).NotEmpty();
            RuleFor(x => x.Subject).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.HtmlContent).NotEmpty();
            RuleFor(x => x.PlainTextContent).NotEmpty();
        }
    }
}