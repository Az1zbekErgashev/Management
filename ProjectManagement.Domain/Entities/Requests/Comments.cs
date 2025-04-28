using ProjectManagement.Domain.Commons;

namespace ProjectManagement.Domain.Entities.Requests
{
    public class Comments : Auditable
    {
        public string? Text { get; set; }
        public Entities.User.User User { get; set; }
        public int UserId { get; set; }
        public Entities.Requests.Request Request { get; set; }
        public int RequestId { get; set; }
        public int? ParentCommentId { get; set; }
        public Comments? ParentComment { get; set; }
        public ICollection<Comments> Replies { get; set; } = new List<Comments>();
    }
}
