
namespace ProjectManagement.Domain.Entities.Task
{
    public class TaskPhotos : Auditable
    {
        public int ImageId { get; set; }
        public virtual required Attachment.Attachment Attachment { get; set; }
        public int TaskId { get; set; }
        public virtual required Task Task { get; set; }
    }
}
