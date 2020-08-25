using System.Collections.Generic;
using MediatR;
using PaperStreet.Logging.Domain.Models;

namespace PaperStreet.Logging.Application.Queries
{
    public class AllAuthenticationLogs
    {
        public class Query : IRequest<List<AuthenticationLog>>
        {
            
        }
        
    }
}