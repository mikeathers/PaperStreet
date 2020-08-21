using PaperStreet.Domain.Core.KeyValuePairs;

namespace PaperStreet.Domain.Core.Events.User
{
    public class UserLoginEvent : UserEvent
    {
        public UserLoginEvent(string userId, string email) : base(userId, email)
        {
            EventDisplayName = EventDisplayNames.UserLoginEvent;
        }
        public override string EventDisplayName { get; }
    }
}