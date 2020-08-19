using System.Collections.Generic;
using MediatR;
using PaperStreet.Logging.Domain.Models;

namespace PaperStreet.Logging.Application.Queries.User
{
    public class AllUserLogs
    {
        public class Query : IRequest<List<AuthenticationLog>>
        {
            
        }
        
    }
}