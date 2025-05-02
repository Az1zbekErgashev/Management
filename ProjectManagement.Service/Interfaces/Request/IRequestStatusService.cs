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
        ValueTask<RequestModel> GetRequestById(int id);
        ValueTask<PagedResult<RequestModel>> GetDeletedRequeststAsync(RequestForFilterDTO dto);
        ValueTask<bool> CreateRequest(RequestForCreateDTO dto);
        ValueTask<bool> DeleteRequest(int id);
        ValueTask<bool> RecoverRequest(int id);
        ValueTask<bool> UpdateRequest(int id, RequestForCreateDTO dto);
        ValueTask<List<RequestsCountModel>> GetRequestsCount();
        ValueTask<List<RequestFilterModel>> GetFilterValue(RequestStatusForFilterDTO dto);
        ValueTask<List<CommentsModel>> GetCommentsAsync(CommentsForFilterDTO dto);
        ValueTask<PagedResult<RequestHistoryModel>> GetRequestHistoryAsync(CommentsForFilterDTO dto);
        ValueTask<bool> CreateComment(CommentForCreateDTO dto);
        ValueTask<bool> UpdateComment(CommentForCreateDTO dto);
        ValueTask<bool> DeleteComment(int commentId);
        ValueTask<List<RequestRateModel>> GetRequestProcent();
        ValueTask<List<RequestCountByStatusModel>> GetStatusCounts();
        ValueTask<List<int>> GetAvailableYears();
        ValueTask<List<Dictionary<string, object>>> GetPieChartData(int? year, int? month);
        ValueTask<List<Dictionary<string, object>>> GetLineChartData(int? year, int? month);
    }
}
