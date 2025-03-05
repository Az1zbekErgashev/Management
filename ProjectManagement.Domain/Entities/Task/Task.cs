using ProjectManagement.Domain.Enum;

namespace ProjectManagement.Domain.Entities.Task;
public class Task : Auditable
{
    public int ProjectId { get; set; }
    public required Projects.Project Project { get; set; }
    public int IssuesFound { get; set; } = 0;
    public DateTime? TotalHourse { get; set; }
    public ICollection<Teams.Team>? Teams { get; set; }
    public ProjectStatus Status { get; set; }
    public ICollection<TaskPhotos>? TaskPhotos { get; set; }
}
