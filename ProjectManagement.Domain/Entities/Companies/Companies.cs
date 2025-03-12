using ProjectManagement.Domain.Commons;
namespace ProjectManagement.Domain.Entities.Companies;
public class Companies : Auditable
{
    public required string CompanyName { get; set; }
    public required string CompanyCode { get; set; }
    public virtual ICollection<Teams.Team>? Teams { get; set; }
    public string? EmployeesCount { get; set; }
    public string? Description { get; set; }
    public string? Site { get; set; }
    public required int CountryId { get; set; }
    public virtual Country.Country Country { get; set; }
}
