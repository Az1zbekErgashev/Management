using ProjectManagement.Domain.Commons;

namespace ProjectManagement.Domain.Entities.Requests
{
    public class RequestStatus : Auditable
    {
        public string? Title { get; set; }
    }
}
