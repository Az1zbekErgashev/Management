
namespace ProjectManagement.Domain.Commons;
public class Auditable
{
    public int Id { get; set; }
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public int IsDeleted { get; set; } = 0;
}
