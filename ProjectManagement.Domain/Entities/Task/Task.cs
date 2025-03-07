﻿using ProjectManagement.Domain.Enum;
using ProjectManagement.Domain.Commons;

namespace ProjectManagement.Domain.Entities.Task;
public class Task : Auditable
{
    public int ProjectId { get; set; }
    public virtual Projects.Project Project { get; set; }
    public int IssuesFound { get; set; } = 0;
    public DateTime? TotalHourse { get; set; }
    public Teams.Team? Team { get; set; }
    public int? TeamId { get; set; }
    public ProjectStatus Status { get; set; }
    public ICollection<TaskPhotos>? TaskPhotos { get; set; }
}
