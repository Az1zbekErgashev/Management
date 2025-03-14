using ProjectManagement.Domain.Entities.Requests;

namespace ProjectManagement.Domain.Models.Request
{
    public class RequestStatusModel 
    {
        public string Title { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int Id { get; set; }

        public virtual RequestStatusModel MapFromEntity(RequestStatus entity)
        {
            Title = entity.Title;
            CreatedAt = entity.CreatedAt;
            UpdatedAt = entity.UpdatedAt;
            Id = entity.Id;
            return this;
        }
    }
}
