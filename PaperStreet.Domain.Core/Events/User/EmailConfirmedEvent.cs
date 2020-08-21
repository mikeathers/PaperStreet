using PaperStreet.Domain.Core.KeyValuePairs;

namespace PaperStreet.Domain.Core.Events.User
{
    public class EmailConfirmedEvent : UserEvent
    {
        public override string EventDisplayName { get; }

        public EmailConfirmedEvent(string userId, string email) : base(userId, email)
        {
            EventDisplayName = EventDisplayNames.EmailConfirmedEvent;
        }
    }
}