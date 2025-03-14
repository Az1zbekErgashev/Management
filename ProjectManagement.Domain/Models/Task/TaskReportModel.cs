using ProjectManagement.Domain.Entities.Task;
using ProjectManagement.Domain.Models.User;

namespace ProjectManagement.Domain.Models.Task
{
    public class TaskReportModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public TimeSpan SpentTime { get; set; }
        public UserModel? User { get; set; }
        public TaskModel? Task { get; set; }
        public List<TaskReportPhotosModel>? TaskReportPhotos { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual TaskReportModel MapFromEntity(TaskReport entity)
        {
            Id = entity.Id;
            CreatedAt = entity.CreatedAt;
            UpdatedAt = entity.UpdatedAt;
            SpentTime = entity.SpentTime;
            TaskReportPhotos = entity.TaskReportPhotos is not null ? entity.TaskReportPhotos.Select(x => new TaskReportPhotosModel().MapFromEntity(x)).ToList() : null;
            Task = entity.Task is not null ? new TaskModel().MapFromEntity(entity.Task) : null;
            Description = entity.Description;
            return this;
        }
    }
}
