using ProjectManagement.Domain.Models.Log;
using ProjectManagement.Domain.Models.PagedResult;
using ProjectManagement.Service.DTOs.Log;

namespace ProjectManagement.Service.Interfaces.Log;
public interface ILogService
{
    ValueTask<PagedResult<LogsModel>> GetAsync(LogsForFilterDTO dto);
}
