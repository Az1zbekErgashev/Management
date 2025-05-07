
using ProjectManagement.Domain.Models.PagedResult;
using ProjectManagement.Domain.Models.Request;
using ProjectManagement.Service.DTOs.Request;
using ProjectManagement.Service.Service.Request;

namespace ProjectManagement.Service.Interfaces.Request
{
    public interface IProcessingStatusService
    {
        ValueTask<bool> CreateAsync(ProcessingStatusDTO dto);
        ValueTask<bool> UpdateAsync(ProcessingStatusDTO dto);
        ValueTask<bool> DeleteAsync(int id);
        ValueTask<bool> DeleteListAsync(List<int> id);
        ValueTask<PagedResult<ProcessingStatusModel>> GetAllAsync(ProcessingStatusFilter dto);
        ValueTask<ProcessingStatusModel> GetByIdAsync(int id);
    }
}
