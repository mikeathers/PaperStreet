using PaperStreet.Domain.Core.KeyValuePairs;

namespace PaperStreet.Domain.Core.Events.User
{
    public class UserLoginEvent : UserEvent
    {
        public UserLoginEvent(string userId, string email)
        {
            UserId = userId;
            Email = email;
            EventDisplayName = EventDisplayNames.UserLoginEvent;
        }
    }
}