using PaperStreet.Domain.Core.KeyValuePairs;

namespace PaperStreet.Domain.Core.Events.User.Logging
{
    public class EmailConfirmedEvent : AuthenticationLogEvent
    {
        public EmailConfirmedEvent(string userId) : base(userId)
        {
            LogMessage = LogMessages.EmailConfirmed;
            LogType = LogTypes.EmailConfirmed;
        }
    }
}