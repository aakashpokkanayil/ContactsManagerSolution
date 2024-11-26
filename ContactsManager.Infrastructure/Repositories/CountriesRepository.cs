using ContactsManager.Core.Domain.Entities;
using ContactsManager.Core.Domain.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using PersonData.PersonsContext;

namespace RepositoryProject.CountryRepository
{
    public class CountriesRepository : ICountryRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CountriesRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Country> AddCountry(Country country)
        {

            if (_dbContext != null && _dbContext.Countries != null)
            {
                _dbContext.Countries.Add(country);
                await _dbContext.SaveChangesAsync();
            }
            return country;
        }

        public async Task<List<Country>> GetAllCountries()
        {
            if (_dbContext != null && _dbContext.Countries != null)
            {
                return await _dbContext.Countries.ToListAsync();
            }
            return new List<Country>();
        }

        public async Task<Country?> GetCountryByCountryName(string countryName)
        {
            if (_dbContext != null && _dbContext.Countries != null)
            {
                return await _dbContext.Countries.FirstOrDefaultAsync(x => x.CountryName == countryName);
            }
            return null;
        }

        public async Task<Country?> GetCountryById(Guid? countryId)
        {
            if (_dbContext != null && _dbContext.Countries != null)
            {
                return await _dbContext.Countries.FirstOrDefaultAsync(x => x.CountryId == countryId);
            }
            return null;
        }
    }
}
