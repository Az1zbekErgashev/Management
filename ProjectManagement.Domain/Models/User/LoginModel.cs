using ProjectManagement.Domain.Enum;
using ProjectManagement.Domain.Models.Attachment;

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
        public AttachmentModel? Image { get; set; }

        public virtual LoginModel MapFromEntity(Domain.Entities.User.User entity, Enum.Role? role, string token)
        {
            Email = entity.Email;
            Id = entity.Id;
            Name = entity.Name;
            Surname = entity.Surname;
            Role = role;
            Token = token;
            Image = entity.Image is not null ? new AttachmentModel().MapFromEntity(entity.Image) : null;
            return this;
        }
    }
}
