using System.Collections.Generic;
using System.Threading.Tasks;
using PaperStreet.Logging.Domain.Models;

namespace PaperStreet.Logging.Application.Interfaces
{
    public interface IAuthenticationLogRepository
    {
        Task<List<AuthenticationLog>> GetAllAuthenticationLogs();
        Task<AuthenticationLog> SaveAuthenticationLog(AuthenticationLog authenticationLog);
    }
}