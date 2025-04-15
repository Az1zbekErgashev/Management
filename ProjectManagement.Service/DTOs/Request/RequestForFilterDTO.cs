using ProjectManagement.Domain.Configuration;
using ProjectManagement.Domain.Enum;

namespace ProjectManagement.Service.DTOs.Request
{
    public class RequestForFilterDTO : PaginationParams
    {
        public string? Text { get; set; }
        public int? Category { get; set; }
        public ProjectStatus? Status { get; set; }
        public Priority? Priority { get; set; }
    }
   
}
