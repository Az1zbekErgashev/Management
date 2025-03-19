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
        public string? ProjectDetails { get; set; } // Описание проекта
        public string? Client { get; set; } // Клиент
        public string? ContactNumber { get; set; } // Контактный номер
        public string? Email { get; set; } // Электронная почта
        public string? ProcessingStatus { get; set; } // Статус обработки
        public string? ResponseStatus { get; set; }
        public string? FinalResult { get; set; } // Итоговый результат
        public string? Notes { get; set; } // Примечания (причина итогового результата)
        public int RequestStatusId { get; set; }
        public string? ProjectDescription { get; set; }
        public string? Date { get; set; }
    }
}
