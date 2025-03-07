using ProjectManagement.Domain.Configuration;
using ProjectManagement.Domain.Enum;
namespace ProjectManagement.Service.DTOs.User
{
    public class UserForFilterDTO : PaginationParams
    {
        public string? Text { get; set; }
        public int? TeamLeaderId { get; set; }
        public int? CompanyId { get; set; }
        public Role? Role { get; set; }
        public int? IsDeleted { get; set; }
    }
}
