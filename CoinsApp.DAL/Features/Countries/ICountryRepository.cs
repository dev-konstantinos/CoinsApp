using CoinsApp.DAL.Features.Countries.Models;

namespace CoinsApp.DAL.Features.Countries
{
    public interface ICountryRepository
    {
        Task<CountryData?> CreateAsync(CountryCreateData data);
        Task<IReadOnlyList<CountryData>> GetAllAsync();
        Task<CountryData?> GetByIdAsync(int countryId);
        Task<CountryData?> SetActiveAsync(CountrySetActiveData data);
        Task<CountryData?> UpdateAsync(CountryUpdateData data);
    }
}