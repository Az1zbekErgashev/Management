using ProjectManagement.Domain.Entities.Teams;

namespace ProjectManagement.Domain.Entities.User;
public class User : Auditable
{
    public required string Email { get; set; }
    public required string Name { get; set; }
    public required string Password { get; set; }
    public required string Surname { get; set; }
    public required string PhoneNumber { get; set; }
    public virtual ICollection<TeamMember>? TeamMembers { get; set; }
}
