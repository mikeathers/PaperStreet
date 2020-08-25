using System.Threading.Tasks;
using PaperStreet.Domain.Core.Bus;
using PaperStreet.Domain.Core.Events.User.Logging;
using PaperStreet.Logging.Application.Interfaces;
using PaperStreet.Logging.Domain.Models;

namespace PaperStreet.Logging.Application.EventHandlers
{
    public class AuthenticationLogEventHandler : IEventHandler<AuthenticationLogEvent>
    {
        private readonly IAuthenticationLogRepository _authenticationLogRepository;

        public AuthenticationLogEventHandler(IAuthenticationLogRepository authenticationLogRepository)
        {
            _authenticationLogRepository = authenticationLogRepository;
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

            await _authenticationLogRepository.SaveAuthenticationLog(authenticationLog);
        }
    }
}