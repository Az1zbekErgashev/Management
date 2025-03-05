using ProjectManagement.Domain.Entities.Certificates;
using ProjectManagement.Domain.Entities.Teams;
using ProjectManagement.Domain.Enum;

namespace ProjectManagement.Domain.Entities.Projects;
public class Project : Auditable
{
    public required string ProjectName { get; set; }
    public int AssignedCompanyId { get; set; }
    public virtual required Companies.Companies Companies { get; set; }
    public int? TeamId { get; set; }
    public Team? Team { get; set; }
    public int PartnerId { get; set; }
    public required Partners.Partners Partner { get; set; }
    public int? CertificateId { get; set; }
    public virtual Certificates.Certificates? Certificates { get; set; }
    public virtual ICollection<Task.Task>? Tasks { get; set; }
}
