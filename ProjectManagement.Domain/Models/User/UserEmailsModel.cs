namespace ProjectManagement.Domain.Models.User
{
    public class UserEmailsModel
    {
        public string Email { get; set; }
        public int Id { get; set; }


        public virtual UserEmailsModel MapFromEntity(Domain.Entities.User.User entity)
        {
            Email = entity.Email;
            Id = entity.Id;
            return this;
        }
    }
}
