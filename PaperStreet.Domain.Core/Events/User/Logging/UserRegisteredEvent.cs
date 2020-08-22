using PaperStreet.Domain.Core.KeyValuePairs;

namespace PaperStreet.Domain.Core.Events.User.Logging
{
    public class UserRegisteredEvent : AuthenticationLogEvent
    {
        public UserRegisteredEvent(string userId): base(userId)
        {
            LogMessage = LogMessages.UserRegistered;
            LogType = LogTypes.UserRegistered;
        }
    }
}