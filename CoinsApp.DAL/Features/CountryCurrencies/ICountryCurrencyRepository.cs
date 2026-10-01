using CoinsApp.DAL.Features.CountryCurrencies.Models;

namespace CoinsApp.DAL.Features.CountryCurrencies
{
    public interface ICountryCurrencyRepository
    {
        Task<CountryCurrencyData?> CreateAsync(CountryCurrencyCreateData data);
        Task<IReadOnlyList<CountryCurrencyData>> GetAllAsync();
        Task<IReadOnlyList<CountryCurrencyData>> GetByCountryAsync(int countryId);
        Task<IReadOnlyList<CountryCurrencyData>> GetByCurrencyAsync(int currencyId);
        Task<CountryCurrencyData?> GetByIdAsync(int countryCurrencyId);
        Task<CountryCurrencyData?> SetActiveAsync(CountryCurrencySetActiveData data);
    }
}