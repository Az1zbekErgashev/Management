using ProjectManagement.Domain.Configuration;
using ProjectManagement.Domain.Enum;

namespace ProjectManagement.Domain.Models.Request
{
    public class RequestModel
    {
        public int Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? InquiryType { get; set; } // Тип запроса
        public string? CompanyName { get; set; } // Название компании
        public string? Department { get; set; } // Ответственный отдел
        public string? ResponsiblePerson { get; set; } // Имя ответственного
        public string? InquiryField { get; set; } // Область запроса
        public string? ClientCompany { get; set; } // Компания клиента
        public string? ProjectDetails { get; set; } // Описание проекта
        public string? Client { get; set; } // Клиент
        public string? ContactNumber { get; set; } // Контактный номер
        public string? Email { get; set; } // Электронная почта
        public string? ProcessingStatus { get; set; } // Статус обработки
        public string? FinalResult { get; set; } // Итоговый результат
        public string? Notes { get; set; } // Примечания (причина итогового результата)
        public RequestStatusModel? RequestStatus { get; set; }
        public int IsDeleted { get; set; }
        public string? Date { get; set; }

        public virtual RequestModel MapFromEntity(Domain.Entities.Requests.Request entity)
        {
            return new RequestModel
            {
                Id = entity.Id,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                InquiryType = entity.InquiryType,
                CompanyName = entity.CompanyName,
                Department = entity.Department,
                ResponsiblePerson = entity.ResponsiblePerson,
                InquiryField = entity.InquiryField,
                ClientCompany = entity.ClientCompany,
                ProjectDetails = entity.ProjectDetails,
                Client = entity.Client,
                ContactNumber = entity.ContactNumber,
                Email = entity.Email,
                ProcessingStatus = entity.ProcessingStatus,
                FinalResult = entity.FinalResult,
                Notes = entity.Notes,
                RequestStatus = entity?.RequestStatus != null ? new RequestStatusModel().MapFromEntity(entity.RequestStatus) : null,
                IsDeleted = entity.IsDeleted,
                Date = entity.Date
            };

        }
    }
}
