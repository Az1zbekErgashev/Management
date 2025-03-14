using ProjectManagement.Domain.Commons;
namespace ProjectManagement.Domain.Entities.Partners
{
    public class PersonContact : Auditable
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
    }
}
