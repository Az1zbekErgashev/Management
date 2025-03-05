namespace ProjectManagement.Domain.Entities.Teams;
public class Team : Auditable
{
    public required string Location { get; set; }
    public required string Name { get; set; }
    public int AssignedCompanyId { get; set; }
    public virtual required Companies.Companies Company { get; set; }
    public virtual ICollection<TeamMember>? TeamMembers { get; set; }
    public int? TaskId { get; set; }
    public virtual Task.Task? Task { get; set; }
}
