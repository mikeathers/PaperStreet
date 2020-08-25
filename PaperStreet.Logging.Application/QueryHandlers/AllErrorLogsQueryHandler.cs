using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PaperStreet.Logging.Application.Interfaces;
using PaperStreet.Logging.Application.Queries;
using PaperStreet.Logging.Domain.Models;

namespace PaperStreet.Logging.Application.QueryHandlers
{
    public class AllErrorLogsQueryHandler : IRequestHandler<AllErrorLogsQuery, List<ErrorLog>>
    {
        private readonly IErrorLogRepository _errorLogRepository;

        public AllErrorLogsQueryHandler(IErrorLogRepository errorLogRepository)
        {
            _errorLogRepository = errorLogRepository;
        }

        public async Task<List<ErrorLog>> Handle(AllErrorLogsQuery request, CancellationToken cancellationToken)
        {
            return await _errorLogRepository.GetAllErrorLogs();
        }
    }
}