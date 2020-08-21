namespace PaperStreet.Domain.Core.Events.User.Logging
{
    public class AuthenticationLogEvent : Event
    {
        public string UserId { get; }
        public string LogType { get; }
        public string LogMessage { get; }

        public AuthenticationLogEvent(string userId, UserLogEvent userLogEvent)
        {
            UserId = userId;
            LogMessage = userLogEvent.LogMessage;
            LogType = userLogEvent.LogType;
        }
    }
}