using System;

namespace PaperStreet.Reservations.Domain.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public int NumberInParty { get; set; }
        public bool ConfirmationEmailSent { get; set; }
        public bool Cancelled { get; set; }
    }
}