using ProjectManagement.Domain.Enum;

namespace ProjectManagement.Domain.Entities.Teams;
public class TeamMember : Auditable
{
    public int UserId { get; set; }
    public virtual required User.User User { get; set; }
    public required Role Role { get; set; }
    public int TeamId { get; set; }
    public virtual required Team Team { get; set; }
    public bool IsCurrent { get; set; }
}
