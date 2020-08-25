using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PaperStreet.Logging.Application.Interfaces;
using PaperStreet.Logging.Application.Queries;
using PaperStreet.Logging.Domain.Models;

namespace PaperStreet.Logging.Application.QueryHandlers
{
    public class AllAuthenticationLogs : IRequestHandler<AllAuthenticationLogsQuery, List<AuthenticationLog>>
    {
        private readonly IAuthenticationLogRepository _authenticationLogRepository;

        public AllAuthenticationLogs(IAuthenticationLogRepository authenticationLogRepository)
        {
            _authenticationLogRepository = authenticationLogRepository;
        }


        public async Task<List<AuthenticationLog>> Handle(Queries.AllAuthenticationLogsQuery request, 
            CancellationToken cancellationToken)
        {
            return await _authenticationLogRepository.GetAllAuthenticationLogs();
        }
    }
}