using ProjectManagement.Domain.Models.Company;
using ProjectManagement.Domain.Models.Task;

namespace ProjectManagement.Domain.Models.Team
{
    public class TeamModel
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public string Name { get; set; }
        public List<TeamMemberModel>? TeamMembers { get; set; }
        public List<TaskModel>? Task { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }


        public virtual TeamModel MapFromEntity(Domain.Entities.Teams.Team entity)
        {
            Id = entity.Id;
            Location = entity.Location;
            TeamMembers = entity.TeamMembers is not null ? entity.TeamMembers.Select(x => new TeamMemberModel().MapFromEntity(x)).ToList() : null;
            CreatedAt = entity.CreatedAt;
            UpdatedAt = entity.UpdatedAt;
            Task = entity.Task is not null ? entity.Task.Select(x => new TaskModel().MapFromEntity(x)).ToList() : null;
            return this;
        }
    }
}
