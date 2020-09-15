using PaperStreet.Domain.Core.KeyValuePairs;

namespace PaperStreet.Domain.Core.Events.User.Logging
{
    public class UpdatedRefreshTokenEvent : AuthenticationLogEvent
    {
        public UpdatedRefreshTokenEvent(string userId) : base(userId)
        {
            LogMessage = LogMessages.UpdatedRefreshToken;
            LogType = LogTypes.UpdatedRefreshToken;
        }
    }
}