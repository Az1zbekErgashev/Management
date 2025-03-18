
using ProjectManagement.Domain.Configuration;

namespace ProjectManagement.Service.DTOs.MultilingualText
{
    public class MultilingualForFilterDTO : PaginationParams
    {
        public string? Text { get; set; }
        public int? IsDeleted { get; set; }
    }
}
