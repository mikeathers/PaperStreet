using PaperStreet.Domain.Core.Models;

namespace PaperStreet.Domain.Core.Events.User.Communication
{
    public class SendEmailEvent : Event
    {
        public Email Email { get; }
        public SendEmailEvent(Email email)
        {
            Email = email;
        }
    }
}