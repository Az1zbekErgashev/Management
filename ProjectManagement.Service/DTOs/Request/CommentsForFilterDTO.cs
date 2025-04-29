using ProjectManagement.Domain.Configuration;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Service.DTOs.Request
{
    public class CommentsForFilterDTO : PaginationParams
    {
        [Required]
        public int RequestId { get; set; }
        public int RepliesPageSize { get; set; } = 10;
    }
}
