using System.Threading.Tasks;
using PaperStreet.Domain.Core.Bus;
using PaperStreet.Domain.Core.Events.User.Logging;
using PaperStreet.Logging.Application.Interfaces;
using PaperStreet.Logging.Domain.Models;

namespace PaperStreet.Logging.Application.EventHandlers.User
{
    public class AuthenticationLogEventHandler : IEventHandler<AuthenticationLogEvent>
    {
        private readonly ILoggingRepository _loggingRepository;

        public AuthenticationLogEventHandler(ILoggingRepository loggingRepository)
        {
            _loggingRepository = loggingRepository;
        }

        public async Task Handle(AuthenticationLogEvent logEvent)
        {
            var authenticationLog = new AuthenticationLog
            {
                UserId = logEvent.UserId,
                MessageType = logEvent.MessageType,
                Timestamp = logEvent.Timestamp,
                LogMessage = logEvent.LogMessage,
                LogType = logEvent.LogType
            };

            await _loggingRepository.SaveAuthenticationLog(authenticationLog);
        }
    }
}