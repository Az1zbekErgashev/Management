using ProjectManagement.Domain.Models.Country;

namespace ProjectManagement.Service.Interfaces.Country
{
    public interface ICountryService
    {
        ValueTask<List<CountryModel>> GetAsync();
        ValueTask<bool> DeleteAsync(int id);
    }
}
