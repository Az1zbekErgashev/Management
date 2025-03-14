using ProjectManagement.Domain.Models.Attachment;
using ProjectManagement.Domain.Models.Company;
using ProjectManagement.Domain.Models.Project;
using ProjectManagement.Domain.Models.User;

namespace ProjectManagement.Domain.Models.Cerificate
{
    public class CertificateModel
    {
        public int Id { get; set; }
        public UserModel? User { get; set; }
        public CompanyModel? Companies { get; set; }
        public AttachmentModel? Image { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }


        public virtual CertificateModel MapFromEntity(Domain.Entities.Certificates.Certificates entity)
        {
            Id = entity.Id;
            Image = entity.Image is not null ? new AttachmentModel().MapFromEntity(entity.Image) : null;
            CreatedAt = entity.CreatedAt;
            UpdatedAt = entity.UpdatedAt;
            Companies = entity.Companies is not null ? new CompanyModel().MapFromEntity(entity.Companies) : null;
            User = entity.User is not null ? new UserModel().MapFromEntity(entity.User) : null;
            return this;
        }
    }
}
