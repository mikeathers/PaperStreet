using PaperStreet.Domain.Core.KeyValuePairs;

namespace PaperStreet.Domain.Core.Events.Errors
{
    public class EmailFailedToSendEvent : ErrorEvent
    {
        public EmailFailedToSendEvent(string userId) :base(userId)
        {
            Message = ErrorMessages.EmailFailedToSend;
        }
    }
}