using ProjectManagement.Domain.Entities.Partners;
using ProjectManagement.Domain.Models.Attachment;
using ProjectManagement.Domain.Models.Country;

namespace ProjectManagement.Domain.Models.Partner
{
    public class PartnerModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CompanyCode { get; set; }
        public CountryModel? Location { get; set; }
        public AttachmentModel? Image { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? Site { get; set; }
        public string? Description { get; set; }
        public string? EmployeesCount { get; set; }

        public virtual PartnerModel MapFromEntity(Partners entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            CompanyCode = entity.CompanyCode;
            Location = entity.Country is not null ? new CountryModel().MapFromEntity(entity.Country) : null;
            Image = entity.Image is not null ? new AttachmentModel().MapFromEntity(entity.Image) : null;
            CreatedAt = entity.CreatedAt;
            UpdatedAt = entity.UpdatedAt;
            Site = entity.Site;
            Description = entity.Description;
            EmployeesCount = entity.EmployeesCount;
            return this;
        }
    }
}
