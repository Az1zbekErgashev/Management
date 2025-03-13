using ProjectManagement.Domain.Commons;
using ProjectManagement.Domain.Enum;

namespace ProjectManagement.Domain.Entities.Logs;
public class Logs : Auditable
{
    public int UserId { get; set; }
    public virtual User.User User { get; set; }
    public string Ip { get; set; }
    public required LogAction Action { get; set; }
}
