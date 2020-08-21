using PaperStreet.Domain.Core.Events.User.Logging;
using PaperStreet.Domain.Core.KeyValuePairs;
using PaperStreet.Domain.Core.Models;

namespace PaperStreet.Domain.Core.Events.User.Logging
{
    public class EmailConfirmationSentEvent : UserLogEvent
    {
        public EmailConfirmationSentEvent()
        {
            LogMessage = "Confirmation email sent to user";
            LogType = "EmailConfirmationSent";
        }
        
        public sealed override string LogType { get; set; }
        public sealed override string LogMessage { get; set; }
    }
}