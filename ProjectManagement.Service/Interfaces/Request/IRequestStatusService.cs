using Microsoft.AspNetCore.Http;
using ProjectManagement.Domain.Enum;
using ProjectManagement.Domain.Models.PagedResult;
using ProjectManagement.Domain.Models.Request;
using ProjectManagement.Service.DTOs.Request;
using ProjectManagement.Service.Service.Requests;
using System.ComponentModel.DataAnnotations;

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
        ValueTask<Dictionary<string, object>> GetLineByStatusChartData(int? year, string status);
        ValueTask<bool> HardDeleteDeletedRequest(List<int> ids);
        ValueTask<bool> SoftDeleteOpenRequest(List<int> ids);
        ValueTask<bool> SoftRecoverOpenRequest(List<int> ids);
        ValueTask<bool> UploadFile(CreateUploadData dto);
        ValueTask<string> GetUploadedFile(int id);
        ValueTask<List<RequestCountByStatusModel>> GetProccesingStatusCounts();
    }
}
