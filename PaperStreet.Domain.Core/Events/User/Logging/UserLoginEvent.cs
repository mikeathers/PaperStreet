using PaperStreet.Domain.Core.KeyValuePairs;

namespace PaperStreet.Domain.Core.Events.User.Logging
{
    public class UserLoginEvent : AuthenticationLogEvent
    {
        public UserLoginEvent(string userId): base(userId)
        {
            LogMessage = LogMessages.UserLoggedIn;
            LogType = LogTypes.UserLoggedIn;
        }
    }
}