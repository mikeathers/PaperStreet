using System.Threading.Tasks;
using FluentValidation;
using PaperStreet.Communication.Application.Interfaces;
using PaperStreet.Domain.Core.Bus;
using PaperStreet.Domain.Core.Events.User.Communication;
using PaperStreet.Domain.Core.Models;

namespace PaperStreet.Communication.Application.EventHandlers
{
    public class SendEmailEventHandler : IEventHandler<SendEmailEvent>
    {
        private readonly IEmailSender _emailSender;

        public SendEmailEventHandler(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public Task Handle(SendEmailEvent @event)
        {
            _emailSender.SendEmail(@event.Email);
            return Task.CompletedTask;
        }
    }
    
    public class EventValidator : AbstractValidator<Email>
    {
        public EventValidator()
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