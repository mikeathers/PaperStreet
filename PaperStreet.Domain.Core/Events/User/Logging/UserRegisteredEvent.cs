using PaperStreet.Domain.Core.KeyValuePairs;

namespace PaperStreet.Domain.Core.Events.User.Logging
{
    public class UserRegisteredEvent : UserLogEvent
    {
        public UserRegisteredEvent()
        {
            LogMessage = "User registered";
            LogType = "UserRegistered";
        }
        
        public sealed override string LogType { get; set; }
        public sealed override string LogMessage { get; set; }
    }
}