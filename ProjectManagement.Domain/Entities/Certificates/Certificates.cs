using ProjectManagement.Domain.Commons;

namespace ProjectManagement.Domain.Entities.Certificates;
public class Certificates : Auditable
{
    public int ProjectId { get; set; }
    public virtual required Projects.Project Project { get; set; }
    public int IssuedByUser { get; set; }
    public virtual required User.User User { get; set; }
    public virtual required Companies.Companies Companies { get; set; }
    public int IssuerToCompanies { get; set; }
    public virtual Attachment.Attachment? Image { get; set; }
    public int? ImageId { get; set; }
}
