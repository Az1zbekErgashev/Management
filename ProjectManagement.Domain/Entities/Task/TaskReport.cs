namespace ProjectManagement.Domain.Entities.Task;
public class TaskReport : Auditable
{
    public int UserId { get; set; }
    public int TaskId { get; set; }
    public required string Description { get; set; }
    public TimeSpan SpentTime { get; set; }
    public virtual required User.User User { get; set; }
    public virtual required Task Task { get; set; }
    public virtual ICollection<TaskReportPhotos>? TaskReportPhotos { get; set; }
}
