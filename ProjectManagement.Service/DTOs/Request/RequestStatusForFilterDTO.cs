using ProjectManagement.Domain.Enum;
namespace ProjectManagement.Service.DTOs.Request
{
    public class RequestStatusForFilterDTO
    {
        public int IsDeleted { get; set; } = 0;
        public ProjectStatus? Status { get; set; }
    }
}
