using PaperStreet.Domain.Core.KeyValuePairs;

namespace PaperStreet.Domain.Core.Events.User
{
    public class UserRegisteredEvent : UserEvent
    {
        public UserRegisteredEvent(string userId, string email) : base(userId, email)
        {
            EventDisplayName = EventDisplayNames.UserRegisteredEvent;
        }

        public override string EventDisplayName { get; }
    }
}