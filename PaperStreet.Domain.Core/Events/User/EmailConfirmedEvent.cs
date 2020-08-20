using PaperStreet.Domain.Core.KeyValuePairs;

namespace PaperStreet.Domain.Core.Events.User
{
    public class EmailConfirmedEvent : UserEvent
    {
        public EmailConfirmedEvent(string userId, string email)
        {
            UserId = userId;
            Email = email;
            EventDisplayName = EventDisplayNames.EmailConfirmedEvent;
        }
    }
}