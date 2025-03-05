namespace ProjectManagement.Domain.Entities.Logs;
public class Logs : Auditable
{
    public int UserId { get; set; }
    public virtual required User.User User { get; set; }
    public long Ip { get; set; }
    public required string Action { get; set; }
}
