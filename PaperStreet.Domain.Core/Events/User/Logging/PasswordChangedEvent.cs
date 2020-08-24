using PaperStreet.Domain.Core.KeyValuePairs;

namespace PaperStreet.Domain.Core.Events.User.Logging
{
    public class PasswordChangedEvent : AuthenticationLogEvent
    {
        public PasswordChangedEvent(string userId) : base(userId)
        {
            LogMessage = LogMessages.ResetPassword;
            LogType = LogTypes.ResetPassword;
        }
    }
}