namespace ProjectManagement.Domain.Models.User
{
    public class CompanyNameModel
    {
        public string CompanyName { get; set; }
        public int CompanyId { get; set; }


        public virtual CompanyNameModel MapFromEntity(string name, int id)
        {
            CompanyName = name;
            CompanyId = id;
            return this;
        }
    }
}
