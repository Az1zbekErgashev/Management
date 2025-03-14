using ProjectManagement.Domain.Commons;

namespace ProjectManagement.Domain.Entities.Partners;
public class Partners : Auditable
{
    public required string Name { get; set; }
    public required string CompanyCode { get; set; }
    public required int CountryId { get; set; }
    public virtual Country.Country Country { get; set; }
    public virtual Attachment.Attachment? Image { get; set; }
    public int? ImageId { get; set; }
    public string? Site { get; set; }
    public string? EmployeesCount { get; set; }
    public string? Description { get; set; }
};
