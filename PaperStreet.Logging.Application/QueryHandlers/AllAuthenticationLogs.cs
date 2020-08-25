using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PaperStreet.Logging.Application.Interfaces;
using PaperStreet.Logging.Domain.Models;

namespace PaperStreet.Logging.Application.QueryHandlers
{
    public class AllAuthenticationLogs : IRequestHandler<Queries.AllAuthenticationLogs.Query, List<AuthenticationLog>>
    {
        private readonly IAuthenticationLogRepository _authenticationLogRepository;

        public AllAuthenticationLogs(IAuthenticationLogRepository authenticationLogRepository)
        {
            _authenticationLogRepository = authenticationLogRepository;
        }


        public async Task<List<AuthenticationLog>> Handle(Queries.AllAuthenticationLogs.Query request, 
            CancellationToken cancellationToken)
        {
            return await _authenticationLogRepository.GetAllAuthenticationLogs();
        }
    }
}