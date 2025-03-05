using ProjectManagement.Domain.Entities.Task;
using ProjectManagement.Domain.Models.Attachment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Domain.Models.Task
{
    public class TaskReportPhotosModel
    {
        public int Id { get; set; }
        public AttachmentModel? Image { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual TaskReportPhotosModel MapFromEntity(TaskReportPhotos entity)
        {
            Id = entity.Id;
            Image = entity.Attachment is not null ? new AttachmentModel().MapFromEntity(entity.Attachment) : null;
            CreatedAt = entity.CreatedAt;
            UpdatedAt = entity.UpdatedAt;
            return this;
        }
    }
}
