using ProjectManagement.Domain.Entities.Logs;
using ProjectManagement.Domain.Enum;

namespace ProjectManagement.Domain.Models.Log
{
    public class LogsModel
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public LogAction Action { get; set; }
        public long? IpAddress { get; set; }


        public virtual LogsModel MapFromEntity(Logs entity)
        {
            Id = entity.Id;
            CreatedAt = entity.CreatedAt;
            UpdatedAt = entity.UpdatedAt;
            Email = entity.User.Email;
            FullName = entity.User.Name + " " + entity.User.Surname;
            PhoneNumber = entity.User.PhoneNumber;
            Action = entity.Action;
            IpAddress = entity.Ip;
            return this;
        }
    }
}
