using ProjectManagement.Domain.Enum;
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
        ValueTask<PagedResult<RequestModel>> GetDeletedRequeststAsync(RequestForFilterDTO dto);
        ValueTask<PagedResult<RequestModel>> GetPendingRequeststAsync(RequestForFilterDTO dto);
        ValueTask<string> CreateRequestAsync(int RequestStatusId);
        ValueTask<bool> CreateRequest(RequestForCreateDTO dto);
        ValueTask<bool> DeleteRequest(int id);
        ValueTask<bool> RecoverRequest(int id);
        ValueTask<bool> ChangeRequestStatus(int id, bool status);
        ValueTask<bool> UpdateRequest(int id, RequestForCreateDTO dto);
        ValueTask<List<RequestFilterModel>> GetFilterValue(RequestStatusForFilterDTO dto);
    }
}
