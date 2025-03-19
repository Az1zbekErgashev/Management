using ProjectManagement.Domain.Configuration;

namespace ProjectManagement.Service.DTOs.MultilingualText
{
    public class UIContentGetAllAndSearchDTO : PaginationParams
    {
        public string? Key { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
