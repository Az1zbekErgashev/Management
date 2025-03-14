using ProjectManagement.Domain.Enum;
using ProjectManagement.Domain.Models.Project;
using ProjectManagement.Domain.Models.Team;

namespace ProjectManagement.Domain.Models.Task
{
    public class TaskModel
    {
        public int Id { get; set; }
        public int IssuesFound { get; set; } = 0;
        public DateTime? TotalHourse { get; set; }
        public ProjectStatus Status { get; set; }
        public ICollection<TaskPhotosModel>? TaskPhotos { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual TaskModel MapFromEntity(Entities.Task.Task entity)
        {
            Id = entity.Id;
            IssuesFound = entity.IssuesFound;
            TotalHourse = entity.TotalHourse;
            Status = entity.Status;
            TaskPhotos = entity.TaskPhotos is not null ? entity.TaskPhotos.Select(x => new TaskPhotosModel().MapFromEntity(x)).ToList() : null;
            CreatedAt = entity.CreatedAt;
            UpdatedAt = entity.UpdatedAt;
            return this;
        }
    }
}
