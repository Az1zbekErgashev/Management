using ProjectManagement.Domain.Entities.Requests;

namespace ProjectManagement.Domain.Models.Request
{
    public class ProcessingStatusModel
    {
        public int Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string Color { get; set; }
        public string Text { get; set; }
        public virtual ProcessingStatusModel MapFromEntity(ProcessingStatus entity)
        {
            Id = entity.Id;
            CreatedAt = entity.CreatedAt;
            Color = entity.Color;
            Text = entity.Text;
            return this;
        }
    }
}
