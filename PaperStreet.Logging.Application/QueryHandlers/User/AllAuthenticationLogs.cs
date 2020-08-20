using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PaperStreet.Logging.Application.Interfaces;
using PaperStreet.Logging.Domain.Models;

namespace PaperStreet.Logging.Application.QueryHandlers.User
{
    public class AllAuthenticationLogs : IRequestHandler<Queries.User.AllAuthenticationLogs.Query, List<AuthenticationLog>>
    {
        private readonly ILoggingRepository _loggingRepository;

        public AllAuthenticationLogs(ILoggingRepository loggingRepository)
        {
            _loggingRepository = loggingRepository;
        }


        public async Task<List<AuthenticationLog>> Handle(Queries.User.AllAuthenticationLogs.Query request, 
            CancellationToken cancellationToken)
        {
            return await _loggingRepository.GetAllAuthenticationLogs();
        }
    }
}