using System;
using System.Threading.Tasks;
using PaperStreet.Domain.Core.Bus;
using PaperStreet.Domain.Core.Events.User;
using PaperStreet.Logging.Application.Interfaces;
using PaperStreet.Logging.Domain.Models;

namespace PaperStreet.Logging.Application.EventHandlers.User
{
    public class UserRegisteredEventHandler : IEventHandler<UserRegisteredEvent>
    {
        private readonly ILoggingRepository _loggingRepository;

        public UserRegisteredEventHandler(ILoggingRepository loggingRepository)
        {
            _loggingRepository = loggingRepository;
        }

        public async Task Handle(UserRegisteredEvent @event)
        {
            var authenticationLog = new AuthenticationLog
            {
                DisplayName = @event.EventDisplayName,
                UserId = @event.UserId,
                MessageType = @event.MessageType,
                Timestamp = @event.Timestamp,
                Email = @event.Email
            };

            await _loggingRepository.SaveAuthenticationLog(authenticationLog);
        }
    }
}