using ProjectManagement.Domain.Entities.Companies;
using ProjectManagement.Domain.Models.Country;
using ProjectManagement.Domain.Models.Team;

namespace ProjectManagement.Domain.Models.Company
{
    public class CompanyModel
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public CountryModel? Location { get; set; }
        public string CompanyCode { get; set; }
        public List<TeamModel>? Teams { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Site { get; set; }
        public string Description { get; set; }
        public string EmployeesCount { get; set; }

        public virtual CompanyModel MapFromEntity(Companies entity)
        {
            Id = entity.Id;
            CompanyName = entity.CompanyName;
            CompanyCode = entity.CompanyCode;
            Location = entity.Country is not null ? new CountryModel().MapFromEntity(entity.Country) : null;
            CreatedAt = entity?.CreatedAt;
            UpdatedAt = entity.UpdatedAt;
            Teams = entity.Teams is not null && entity.Teams.Any() ? entity.Teams.Select(x => new TeamModel().MapFromEntity(x)).ToList() : null;
            Site = entity.Site;
            Description = entity.Description;
            EmployeesCount = entity.EmployeesCount;
            return this;
        }
    }
}
