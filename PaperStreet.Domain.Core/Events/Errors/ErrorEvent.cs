namespace PaperStreet.Domain.Core.Events.Errors
{
    public class ErrorEvent : Event
    {
        public string UserId { get; set; }
        public string Message { get; set; }
    }
}