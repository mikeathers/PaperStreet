namespace PaperStreet.Domain.Core.Events.User
{
    public abstract class UserEvent : Event
    {
        protected UserEvent(string userId, string email)
        {
            UserId = userId;
            Email = email;
        }

        public string UserId { get; }
        public string Email { get; }
        public abstract string EventDisplayName { get; }
    }
}