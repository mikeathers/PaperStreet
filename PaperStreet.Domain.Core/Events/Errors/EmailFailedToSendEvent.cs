namespace PaperStreet.Domain.Core.Events.Errors
{
    public class EmailFailedToSendEvent : ErrorEvent
    {
        public EmailFailedToSendEvent(string userId, string message)
        {
            UserId = userId;
            Message = message;
        }
    }
}