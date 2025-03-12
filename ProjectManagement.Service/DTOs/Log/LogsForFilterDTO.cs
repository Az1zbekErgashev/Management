using ProjectManagement.Domain.Configuration;
using ProjectManagement.Domain.Enum;

namespace ProjectManagement.Service.DTOs.Log
{
    public class LogsForFilterDTO : PaginationParams
    {
        public int? UserId { get; set; }
        public LogAction? Action { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Text{ get; set; }
}
}
