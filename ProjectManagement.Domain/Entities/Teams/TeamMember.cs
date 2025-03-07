using ProjectManagement.Domain.Enum;
using ProjectManagement.Domain.Commons;
using ProjectManagement.Domain.Entities.User;

namespace ProjectManagement.Domain.Entities.Teams;
public class TeamMember : Auditable
{
    public int TeamId { get; set; }
    public virtual Team Team { get; set; }

    public int UserId { get; set; }
    public virtual User.User User { get; set; }
}
