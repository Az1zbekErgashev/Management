namespace ProjectManagement.Domain.Entities.Companies;
public class Companies : Auditable
{
    public required string CompanyName { get; set; }
    public required string Location { get; set; }
    public required string CompanyCode { get; set; }
}
