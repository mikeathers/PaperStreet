using System;
using System.Threading.Tasks;
using PaperStreet.Domain.Core.Bus;
using PaperStreet.Domain.Core.Events.User;
using PaperStreet.Logging.Data.Context;
using PaperStreet.Logging.Domain.Models;

namespace PaperStreet.Logging.Application.EventHandlers.User
{
    public class UserRegisteredEventHandler : IEventHandler<UserRegisteredEvent>
    {
        private readonly LoggingDbContext _context;

        public UserRegisteredEventHandler(LoggingDbContext context)
        {
            _context = context;
        }

        public Task Handle(UserRegisteredEvent @event)
        {
            var authenticationLog = new AuthenticationLog
            {
                DisplayName = @event.DisplayName,
                UserId = @event.UserId,
                MessageType = @event.MessageType,
                Timestamp = @event.Timestamp,
                Email = @event.Email
            };
            
            _context.AuthenticationLogs.Add(authenticationLog);
            var success = _context.SaveChanges() > 0;
            
            if (success)
                return Task.CompletedTask;
            
            throw new Exception("Problem saving the Log");
        }
    }
}