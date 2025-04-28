using ProjectManagement.Domain.Commons;
using ProjectManagement.Domain.Enum;

namespace ProjectManagement.Domain.Entities.Requests
{
    public class RequestHistory : Auditable
    {
        public Entities.User.User User { get; set; }
        public int UserId { get; set; }
        public Entities.Requests.Request Request { get; set; }
        public int RequestId { get; set; }
        public RequestLog Log { get; set; }
        public RequestLogType Type { get; set; }
    }
}
