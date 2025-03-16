using ProjectManagement.Domain.Enum;
using ProjectManagement.Domain.Models.Attachment;
using ProjectManagement.Domain.Models.Country;

namespace ProjectManagement.Domain.Models.User
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Role { get; set; }
        public int IsDeleted { get; set; }
        public CountryModel? Country { get; set; }
        public AttachmentModel? Image { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Role? RoleId { get; set; }

        public virtual UserModel MapFromEntity(Domain.Entities.User.User entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Email = entity.Email;
            Surname = entity.Surname;
            PhoneNumber = entity.PhoneNumber;
            CreatedAt = entity.CreatedAt;
            UpdatedAt = entity.UpdatedAt;
            Role = GetUserRole(entity?.IndividualRole);
            RoleId = entity?.IndividualRole;
            IsDeleted = entity.IsDeleted;
            Country = entity.Country is not null ? new CountryModel().MapFromEntity(entity.Country) : null;
            Image = entity.Image is not null ? new AttachmentModel().MapFromEntity(entity.Image) : null;
            DateOfBirth = entity.DateOfBirth;
            return this;
        }

        private string GetUserRole(Role? num)
        {
            switch (num)
            {
                case Enum.Role.Employees:
                    return "employees";
                case Enum.Role.SuperAdmin:
                    return "admin";
                default:
                    return "employees";
            }
        }
    }
}
