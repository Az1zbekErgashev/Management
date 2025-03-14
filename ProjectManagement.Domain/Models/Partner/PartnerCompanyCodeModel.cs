using ProjectManagement.Domain.Entities.Partners;

namespace ProjectManagement.Domain.Models.Partner
{
    public class PartnerCompanyCodeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CompanyCode { get; set; }


        public virtual PartnerCompanyCodeModel MapFromEntity(Partners entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            CompanyCode = entity.CompanyCode;
            return this;
        }
    }
}
