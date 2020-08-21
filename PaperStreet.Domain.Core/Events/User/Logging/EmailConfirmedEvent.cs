using PaperStreet.Domain.Core.KeyValuePairs;

namespace PaperStreet.Domain.Core.Events.User.Logging
{
    public class EmailConfirmedEvent : UserLogEvent
    {
        public EmailConfirmedEvent()
        {
            LogMessage = "User confirmed email";
            LogType = "EmailConfirmed";
        }
        public sealed override string LogType { get; set; }
        public sealed override string LogMessage { get; set; }
    }
}