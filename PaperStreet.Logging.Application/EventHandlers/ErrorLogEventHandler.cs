using System.Threading.Tasks;
using PaperStreet.Domain.Core.Bus;
using PaperStreet.Domain.Core.Events.Errors;
using PaperStreet.Logging.Application.Interfaces;
using PaperStreet.Logging.Domain.Models;

namespace PaperStreet.Logging.Application.EventHandlers
{
    public class ErrorLogEventHandler : IEventHandler<ErrorLogEvent>
    {
        private readonly IErrorLogRepository _errorLogRepository;

        public ErrorLogEventHandler(IErrorLogRepository errorLogRepository)
        {
            _errorLogRepository = errorLogRepository;
        }

        public async Task Handle(ErrorLogEvent @event)
        {
            var errorToLog = new ErrorLog
            {
                UserId = @event.UserId,
                ErrorMessage = @event.Message,
                MessageType = @event.MessageType,
                Timestamp = @event.Timestamp
            };

            await _errorLogRepository.SaveErrorLog(errorToLog);
        }
    }
}