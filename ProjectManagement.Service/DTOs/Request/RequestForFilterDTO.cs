using ProjectManagement.Domain.Configuration;

namespace ProjectManagement.Service.DTOs.Request
{
    public class RequestForFilterDTO : PaginationParams
    {
        public string? SortBy { get; set; }
        public string? Order { get; set; }
        public string? Category { get; set; }
        public string? InquiryType { get; set; }
        public string? CompanyName { get; set; }
        public string? Department { get; set; }
        public string? ResponsiblePerson { get; set; }
        public string? InquiryField { get; set; } 
        public string? ClientCompany { get; set; }
        public string? ProjectDetails { get; set; } 
        public string? Client { get; set; } 
        public string? ContactNumber { get; set; } 
        public string? Email { get; set; } 
        public string? ProcessingStatus { get; set; } 
        public string? FinalResult { get; set; } 
        public string? Notes { get; set; }
        public int? RequestStatusId { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
