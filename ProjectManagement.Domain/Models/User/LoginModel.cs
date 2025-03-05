using ProjectManagement.Domain.Enum;

namespace ProjectManagement.Domain.Models.User
{
    public class LoginModel
    {
        public string Email { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int? TeamId { get; set; }
        public Role? Role { get; set; }

        public virtual LoginModel MapFromEntity(Domain.Entities.User.User entity)
        {
            Email = entity.Email;
            Id = entity.Id;
            Name = entity.Name;
            Surname = entity.Surname;
            TeamId = entity?.TeamMembers?.Where(x => x.IsCurrent).FirstOrDefault()?.Team?.Id;
            Role = entity?.TeamMembers?.Where(x => x.IsCurrent)?.FirstOrDefault()?.Role;
            return this;
        }
    }
}
