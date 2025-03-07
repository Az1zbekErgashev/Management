
namespace ProjectManagement.Domain.Commons;
public class Auditable
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int IsDeleted { get; set; } = 0;
}
