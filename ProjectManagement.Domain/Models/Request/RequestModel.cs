﻿using ProjectManagement.Domain.Enum;

namespace ProjectManagement.Domain.Models.Request
{
    public class RequestModel
    {
        public int Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
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
        public RequestStatusModel? RequestStatus { get; set; }
        public int IsDeleted { get; set; }
        public string? Date { get; set; }
        public string? AdditionalInformation { get; set; }
        public string? InquirySource { get; set; }
        public string? ProjectBudget { get; set; }
        public Priority Priority { get; set; }
        public DateTime? Deadline { get; set; }
        public ProjectStatus Status { get; set; }

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
                Date = entity.Date,
                Priority = entity.Priority,
                Deadline = entity.Deadline,
                Status = entity.Status
            };

        }
    }
}
