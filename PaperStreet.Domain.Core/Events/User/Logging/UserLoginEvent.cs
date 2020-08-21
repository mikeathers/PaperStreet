using PaperStreet.Domain.Core.KeyValuePairs;

namespace PaperStreet.Domain.Core.Events.User.Logging
{
    public class UserLoginEvent : UserLogEvent
    {
        public UserLoginEvent()
        {
            LogMessage = "User logged in";
            LogType = "UserLogin";
        }
        public sealed override string LogType { get; set; }
        public sealed override string LogMessage { get; set; }
    }
}