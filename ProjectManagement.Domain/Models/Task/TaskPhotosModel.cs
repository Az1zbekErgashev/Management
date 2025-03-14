using ProjectManagement.Domain.Entities.Task;
using ProjectManagement.Domain.Models.Attachment;

namespace ProjectManagement.Domain.Models.Task
{
    public class TaskPhotosModel
    {
        public int Id { get; set; }
        public AttachmentModel? Image { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual TaskPhotosModel MapFromEntity(TaskPhotos entity)
        {
            Id = entity.Id;
            Image = entity.Attachment is not null ? new AttachmentModel().MapFromEntity(entity.Attachment) : null;
            CreatedAt = entity.CreatedAt;
            UpdatedAt = entity.UpdatedAt;
            return this;
        }
    }
}
