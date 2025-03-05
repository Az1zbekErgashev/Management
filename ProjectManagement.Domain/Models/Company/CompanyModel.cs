using ProjectManagement.Domain.Entities.Companies;
using ProjectManagement.Domain.Models.Team;

namespace ProjectManagement.Domain.Models.Company
{
    public class CompanyModel
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string Location { get; set; }
        public string CompanyCode { get; set; }
        public List<TeamModel>? Teams { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual CompanyModel MapFromEntity(Companies entity)
        {
            Id = entity.Id;
            CompanyName = entity.CompanyName;
            CompanyCode = entity.CompanyCode;
            Location = entity.Location;
            CreatedAt = entity.CreatedAt;
            UpdatedAt = entity.UpdatedAt;
            Teams = entity.Teams is not null && entity.Teams.Any() ? entity.Teams.Select(x => new TeamModel().MapFromEntity(x)).ToList() : null;
            return this;
        }
    }
}
