using ProjectManagement.Domain.Commons;

namespace ProjectManagement.Domain.Entities.Requests
{
    public class ProcessingStatus : Auditable
    {
        public required string Text { get; set; }
        public required string Color { get; set; }
    }
}
