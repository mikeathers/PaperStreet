using PaperStreet.Domain.Core.KeyValuePairs;

namespace PaperStreet.Domain.Core.Events.User.Logging
{
    public class ResetPasswordRequestEvent : AuthenticationLogEvent
    {
        public ResetPasswordRequestEvent(string userId) : base(userId)
        {
            LogMessage = LogMessages.ResetPasswordRequest;
            LogType = LogTypes.ResetPasswordRequest;
        }
    }
}