using System.Collections.Generic;
using MediatR;
using PaperStreet.Logging.Domain.Models;

namespace PaperStreet.Logging.Application.Queries
{
    public class AllErrorLogs
    {
        public class Query : IRequest<List<ErrorLog>>
        {
            
        }
    }
}