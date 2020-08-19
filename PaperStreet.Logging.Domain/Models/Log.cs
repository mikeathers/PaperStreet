using System;

namespace PaperStreet.Logging.Domain.Models
{
    public class Log
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string MessageType { get; set; }
    }
}