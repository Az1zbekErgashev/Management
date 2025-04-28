using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Domain.Models.Request
{
    public class CommentForCreateDTO
    {
        [Required]
        public int RequestId { get; set; }

        [Required]
        public string Text { get; set; }

        public int? CommentId { get; set; }
        public int? ParentCommentId { get; set; }
    }
}