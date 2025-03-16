using ProjectManagement.Domain.Configuration;

namespace ProjectManagement.Service.DTOs.Request
{
    public class RequestForFilterDTO : PaginationParams
    {
        public string? SortBy { get; set; }
        public string? Order { get; set; }
        public List<string>? InquiryType { get; set; }
        public List<string>? Department { get; set; }
        public List<string>? ResponsiblePerson { get; set; }
        public List<string>? InquiryField { get; set; } 
        public List<string>? ClientCompany { get; set; }
        public List<string>? ProjectDetails { get; set; } 
        public List<string>? Client { get; set; } 
        public List<string>? ContactNumber { get; set; } 
        public List<string>? Email { get; set; } 
        public List<string>? ProcessingStatus { get; set; } 
        public List<string>? FinalResult { get; set; } 
        public List<string>? Notes { get; set; }
        public List<string>? Date { get; set; }
        public List<string>? CompanyName { get; set; }
        public int? RequestStatusId { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
