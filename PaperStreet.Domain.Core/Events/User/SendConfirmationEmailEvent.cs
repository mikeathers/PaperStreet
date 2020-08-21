using PaperStreet.Domain.Core.KeyValuePairs;
using PaperStreet.Domain.Core.Models;

namespace PaperStreet.Domain.Core.Events.User
{
    public class SendConfirmationEmailEvent : UserEvent
    {
        public SendConfirmationEmailEvent(string userId, string email, Email emailToSend) : base(userId, email)
        {
            EventDisplayName = EventDisplayNames.ConfirmationEmailSentEvent;
            EmailToSend = emailToSend;
        }

        public override string EventDisplayName { get; }
        
        public Email EmailToSend { get; }
    }
}