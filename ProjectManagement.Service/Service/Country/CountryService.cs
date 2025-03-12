using Microsoft.EntityFrameworkCore;
using ProjectManagement.Domain.Models.Country;
using ProjectManagement.Service.Exception;
using ProjectManagement.Service.Interfaces.Country;
using ProjectManagement.Service.Interfaces.IRepositories;

namespace ProjectManagement.Service.Service.Country
{
    public class CountryService : ICountryService
    {
        private readonly IGenericRepository<Domain.Entities.Country.Country> countryRepository;

        public CountryService(IGenericRepository<Domain.Entities.Country.Country> countryRepository)
        {
            this.countryRepository = countryRepository;
        }

        public async ValueTask<List<CountryModel>> GetAsync()
        {
            var countrys = await countryRepository.GetAll(x => x.IsDeleted == 0).ToListAsync();

            var model = countrys.Select(x => new CountryModel().MapFromEntity(x)).ToList();
            return model;
        }

        public async ValueTask<bool> DeleteAsync(int id)
        {
            var country = await countryRepository.GetAll(x => x.IsDeleted == 0 && x.Id == id).FirstOrDefaultAsync();

            if (country is null) throw new ProjectManagementException(404, "country_not_found");

            country.IsDeleted = 1;
            country.UpdatedAt = DateTime.UtcNow;

            countryRepository.UpdateAsync(country);
            await countryRepository.SaveChangesAsync();
            return true;
        }
    }
}
