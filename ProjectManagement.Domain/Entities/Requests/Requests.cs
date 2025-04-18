﻿using ProjectManagement.Domain.Commons;
using ProjectManagement.Domain.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManagement.Domain.Entities.Requests;
public class Request : Auditable
{
    public string? InquiryType { get; set; } 
    public string? LastUpdated { get; set; }
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
    public RequestStatus RequestStatus { get; set; }
    public int RequestStatusId { get; set; }
    public string? Date { get; set; }
    public string? Status { get; set; }
    public DateTime? Deadline { get; set; }
    public string? Priority { get; set; }
    public string? ProjectBudget { get; set; }
    public string? InquirySource { get; set; }
    public string? AdditionalInformation { get; set; }
    public long? ChatId { get; set; }
}
