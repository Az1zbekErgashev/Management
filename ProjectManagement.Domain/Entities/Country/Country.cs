using ProjectManagement.Domain.Commons;


namespace ProjectManagement.Domain.Entities.Country
{
    public class Country : Auditable
    {
        public required string Name { get; set; }
    }
}
