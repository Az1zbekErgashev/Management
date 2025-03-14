using ProjectManagement.Domain.Models.PagedResult;
using ProjectManagement.Domain.Models.Request;
using ProjectManagement.Service.DTOs.Request;

namespace ProjectManagement.Service.Interfaces.Request
{
    public interface IRequestStatusService
    {
        ValueTask<List<RequestStatusModel>> GetAsync();
        ValueTask<bool> CreateAsync(RequestStatusForCreateDTO dto);
        ValueTask<bool> DeleteAsync(int id);
        ValueTask<bool> UpdateAsync(int id, RequestStatusForCreateDTO dto);
        ValueTask<PagedResult<RequestModel>> GetRequeststAsync(RequestForFilterDTO dto);
        ValueTask<string> CreateRequestAsync(List<RequestForCreateDTO> dto);
        ValueTask<bool> CreateRequest(RequestForCreateDTO dto);
    }
}
