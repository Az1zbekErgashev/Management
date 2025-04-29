using Microsoft.AspNetCore.Http;

namespace ProjectManagement.Service.DTOs.Request
{
    public class RequestForCreateDTO
    {
        public string? Sequence { get; set; }
        public string? InquiryType { get; set; } // Тип запроса
        public string? CompanyName { get; set; } // Название компании
        public string? Department { get; set; } // Ответственный отдел
        public string? ResponsiblePerson { get; set; } // Имя ответственного
        public string? InquiryField { get; set; } // Область запроса
        public string? ClientCompany { get; set; } // Компания клиента
        public string? ProjectDetails { get; set; }
        public string? Client { get; set; } 
        public string? ContactNumber { get; set; }
        public string? Email { get; set; } 
        public string? ProcessingStatus { get; set; } 
        public string? ResponseStatus { get; set; }
        public string? Notes { get; set; } 
        public int RequestStatusId { get; set; }
        public string? ProjectDescription { get; set; }
        public string? Date { get; set; }
        public string? Status { get; set; }
        public IFormFile? File { get; set; }
        public bool UpdateFile { get; set; }
        public DateTime? LastUpdated { get; set; }
        public bool RemoveFile { get; set; } = false;
    }
}
