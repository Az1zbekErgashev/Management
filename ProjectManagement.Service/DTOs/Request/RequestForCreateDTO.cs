using ProjectManagement.Domain.Enum;
using ProjectManagement.Service.Extencions;
using System.Text.Json.Serialization;

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
        public string? FinalResult { get; set; } 
        public string? Notes { get; set; } 
        public int RequestStatusId { get; set; }
        public string? ProjectDescription { get; set; }

        [JsonConverter(typeof(NullableDateTimeConverter))]
        public DateTime? Date { get; set; }
        public ProjectStatus Status { get; set; } = 0;
        public DateTime? Deadline { get; set; }
        public Priority Priority { get; set; } = Priority.Normal;
    }
}
