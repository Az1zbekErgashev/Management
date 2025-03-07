using ProjectManagement.Domain.Enum;

namespace ProjectManagement.Domain.Models.User
{
    public class LoginModel
    {
        public string Email { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public Role? Role { get; set; }
        public string Token { get; set; }

        public virtual LoginModel MapFromEntity(Domain.Entities.User.User entity, Enum.Role? role, string token)
        {
            Email = entity.Email;
            Id = entity.Id;
            Name = entity.Name;
            Surname = entity.Surname;
            Role = role;
            Token = token;
            return this;
        }
    }
}
