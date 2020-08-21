using System.Threading.Tasks;
using PaperStreet.Communication.Application.Interfaces;
using PaperStreet.Domain.Core.Bus;
using PaperStreet.Domain.Core.Events.User.Communication;

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
}