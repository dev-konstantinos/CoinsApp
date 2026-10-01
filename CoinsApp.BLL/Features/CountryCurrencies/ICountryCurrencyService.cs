using CoinsApp.BLL.Features.CountryCurrencies.ViewModels;

namespace CoinsApp.BLL.Features.CountryCurrencies
{
    public interface ICountryCurrencyService
    {
        Task<CountryCurrencyDetailsViewModel?> CreateAsync(CreateCountryCurrencyViewModel model);
        Task<IReadOnlyList<CountryCurrencyListItemViewModel>> GetAllAsync();
        Task<IReadOnlyList<CountryCurrencyListItemViewModel>> GetByCountryAsync(int countryId);
        Task<IReadOnlyList<CountryCurrencyListItemViewModel>> GetByCurrencyAsync(int currencyId);
        Task<CountryCurrencyDetailsViewModel?> GetByIdAsync(int countryCurrencyId);
        Task<CountryCurrencyDetailsViewModel?> SetActiveAsync(SetCountryCurrencyActiveViewModel model);
    }
}