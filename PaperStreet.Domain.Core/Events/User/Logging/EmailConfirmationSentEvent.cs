using PaperStreet.Domain.Core.KeyValuePairs;

namespace PaperStreet.Domain.Core.Events.User.Logging
{
    public class EmailConfirmationSentEvent : AuthenticationLogEvent
    {
        public EmailConfirmationSentEvent(string userId) : base(userId)
        {
            LogMessage = LogMessages.EmailConfirmationSent;
            LogType = LogTypes.EmailConfirmationSent;
        }
    }
}