using ProjectManagement.Domain.Configuration;

namespace ProjectManagement.Service.DTOs.Partner
{
    public class PartnerForFilterDTO : PaginationParams
    {
        public List<int>? Id { get; set; }
        public string? Text { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
