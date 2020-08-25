using System.Collections.Generic;
using System.Threading.Tasks;
using PaperStreet.Logging.Domain.Models;

namespace PaperStreet.Logging.Application.Interfaces
{
    public interface IErrorLogRepository
    {
        Task<ErrorLog> SaveErrorLog(ErrorLog errorLog);

        Task<List<ErrorLog>> GetAllErrorLogs();
    }
}