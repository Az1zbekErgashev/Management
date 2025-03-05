using ProjectManagement.Domain.Entities.Partners;
using ProjectManagement.Domain.Models.Attachment;

namespace ProjectManagement.Domain.Models.Partner
{
    public class PartnerModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyLocation { get; set; }
        public AttachmentModel? Image { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual PartnerModel MapFromEntity(Partners entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            CompanyCode = entity.CompanyCode;
            CompanyLocation = entity.CompanyLocation;
            Image = entity.Image is not null ? new AttachmentModel().MapFromEntity(entity.Image) : null;
            CreatedAt = entity.CreatedAt;
            UpdatedAt = entity.UpdatedAt;
            return this;
        }
    }
}
