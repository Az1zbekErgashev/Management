using ProjectManagement.Domain.Entities.Requests;
using ProjectManagement.Domain.Models.User;

namespace ProjectManagement.Domain.Models.Request
{
    public class CommentsModel
    {
        public UserModel User { get; set; }
        public string Text { get; set; }
        public int Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual CommentsModel MapFromEntity(Comments entity)
        {
            User = new UserModel().MapFromEntity(entity.User);
            Text = entity.Text;
            Id = entity.Id;
            CreatedAt = entity.CreatedAt;
            UpdatedAt = entity.UpdatedAt;
            return this;
        }
    }
}
