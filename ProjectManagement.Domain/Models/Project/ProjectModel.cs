using ProjectManagement.Domain.Models.Cerificate;
using ProjectManagement.Domain.Models.Partner;
using ProjectManagement.Domain.Models.Task;

namespace ProjectManagement.Domain.Models.Project
{
    public class ProjectModel
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public PartnerModel? Partner { get; set; }
        public CertificateModel? Certificates { get; set; }
        public List<TaskModel>? Tasks { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ProjectModel MapFromEntity(Domain.Entities.Projects.Project entity)
        {
            Id = entity.Id;
            ProjectName = entity.ProjectName;
            Tasks = entity?.Tasks is not null ? entity.Tasks.Select(x => new TaskModel().MapFromEntity(x)).ToList() : null;
            CreatedAt = entity.CreatedAt;
            UpdatedAt = entity.UpdatedAt;
            Certificates = entity.Certificates is not null ? new CertificateModel().MapFromEntity(entity.Certificates) : null;
            Partner = entity.Partner is not null ? new PartnerModel().MapFromEntity(entity.Partner) : null;
            return this;
        }
    }
}
