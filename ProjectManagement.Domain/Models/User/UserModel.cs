namespace ProjectManagement.Domain.Models.User
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }


        public virtual UserModel MapFromEntity(Domain.Entities.User.User entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Email = entity.Email;
            Surname = entity.Surname;
            PhoneNumber = entity.PhoneNumber;
            CreatedAt = entity.CreatedAt;
            UpdatedAt = entity.UpdatedAt;
            return this;
        }
    }
}
