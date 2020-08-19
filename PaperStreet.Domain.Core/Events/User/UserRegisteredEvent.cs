using PaperStreet.Domain.Core.KeyValuePairs;

namespace PaperStreet.Domain.Core.Events.User
{
    public class UserRegisteredEvent : UserEvent
    {
        public UserRegisteredEvent(string userId, string email)
        {
            UserId = userId;
            Email = email;
            DisplayName = EventDisplayNames.UserRegisteredEvent;
        }
    }
}