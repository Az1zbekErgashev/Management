namespace ProjectManagement.Domain.Entities.Task;
public class TaskReportPhotos : Auditable
{
    public int ImageId { get; set; }
    public virtual required Attachment.Attachment Attachment { get; set; }
    public int TaskReportId { get; set; }
    public virtual required TaskReport TaskReport { get; set; }
}
