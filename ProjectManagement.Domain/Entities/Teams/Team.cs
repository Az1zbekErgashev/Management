using ProjectManagement.Domain.Commons;

namespace ProjectManagement.Domain.Entities.Teams;
public class Team : Auditable
{
    public required string Location { get; set; }
    public int? AssignedCompanyId { get; set; }
    public virtual Companies.Companies? Company { get; set; }
    public virtual ICollection<TeamMember>? TeamMembers { get; set; }
    public virtual ICollection<Task.Task>? Task { get; set; }
}
