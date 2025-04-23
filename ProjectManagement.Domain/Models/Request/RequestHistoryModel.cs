using ProjectManagement.Domain.Entities.Requests;
using ProjectManagement.Domain.Enum;
using ProjectManagement.Domain.Models.User;

namespace ProjectManagement.Domain.Models.Request
{
    public class RequestHistoryModel
    {
        public UserModel? User { get; set; }
        public RequestLog Log { get; set; }
        public int Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }


        public virtual RequestHistoryModel MapFromEntity(RequestHistory entity)
        {
            User = new UserModel().MapFromEntity(entity.User);
            Log = entity.Log;
            Id = entity.Id;
            CreatedAt = entity.CreatedAt;
            UpdatedAt = entity.UpdatedAt;
            return this;
        }
    }
}
