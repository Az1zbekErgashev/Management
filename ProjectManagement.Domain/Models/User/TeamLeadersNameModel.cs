namespace ProjectManagement.Domain.Models.User
{
    public class TeamLeadersNameModel
    {
        public string FullName { get; set; }
        public int UserId { get; set; }

        public virtual TeamLeadersNameModel MapFromEntity(string fullName, int userId)
        {
            FullName = fullName;
            UserId = userId;
            return this;
        }
    }

}
