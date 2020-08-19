namespace PaperStreet.Domain.Core.Events.User
{
    public class UserEvent : Event
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
    }
}