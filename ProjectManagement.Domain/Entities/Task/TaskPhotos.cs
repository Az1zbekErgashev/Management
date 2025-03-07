using ProjectManagement.Domain.Commons;

namespace ProjectManagement.Domain.Entities.Task
{
    public class TaskPhotos : Auditable
    {
        public int ImageId { get; set; }
        public virtual Attachment.Attachment Attachment { get; set; }
        public int TaskId { get; set; }
        public virtual Task Task { get; set; }
    }
}
