using ContactsManager.Core.Domain.Entities;

namespace ContactsManager.Core.Domain.RepositoryContracts
{
    public interface ICountryRepository
    {
        Task<Country> AddCountry(Country country);
        Task<List<Country>> GetAllCountries();
        Task<Country?> GetCountryById(Guid? countryId);
        Task<Country?> GetCountryByCountryName(string countryName);

    }
}
