namespace PaperStreet.Domain.Core.Events.User.Logging
{
    public class AuthenticationLogEvent : Event
    {
        public string UserId { get; }
        public string LogType { get; set; }
        public string LogMessage { get; set; }

        public AuthenticationLogEvent(string userId)
        {
            UserId = userId;
        }
    }
}