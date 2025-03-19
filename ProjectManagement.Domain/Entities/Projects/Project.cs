
using ProjectManagement.Domain.Commons;
using ProjectManagement.Domain.Entities.Teams;

namespace ProjectManagement.Domain.Entities.Projects;
public class Project : Auditable
{
    public required string ProjectName { get; set; }
    public int AssignedCompanyId { get; set; }
    public virtual Companies.Companies Companies { get; set; }
    public int? TeamId { get; set; }
    public Team? Team { get; set; }
    public int PartnerId { get; set; }
    public required Partners.Partners Partner { get; set; }
    public int? CertificateId { get; set; }
    public virtual Certificates.Certificates? Certificates { get; set; }
    public virtual ICollection<Task.Task>? Tasks { get; set; }
}
