using ProjectManagement.Domain.Enum;
using ProjectManagement.Domain.Models.User;

namespace ProjectManagement.Domain.Models.Team
{
    public class TeamMemberModel
    {
        public int Id { get; set; }
        public UserModel? User { get; set; }
        public Role Role { get; set; }
        public bool IsCurrent { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual TeamMemberModel MapFromEntity(Domain.Entities.Teams.TeamMember entity)
        {
            Id = entity.Id;
            User = entity.User is not null ? new UserModel().MapFromEntity(entity.User) : null;
            Role = entity.Role;
            IsCurrent = entity.IsCurrent;
            CreatedAt = entity.CreatedAt;
            UpdatedAt = entity.UpdatedAt;
            return this;
        }
    }
}
