namespace ProjectManagement.Domain.Models.Country
{
    public class CountryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual CountryModel MapFromEntity(Entities.Country.Country entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            return this;
        }
    }
}
