using CoinsApp.BLL.Countries.ViewModels;

namespace CoinsApp.BLL.Countries
{
    public interface ICountryService
    {
        Task<CountryDetailsViewModel?> CreateAsync(CreateCountryViewModel model);
        Task<IReadOnlyList<CountryListItemViewModel>> GetAllAsync();
        Task<CountryDetailsViewModel?> GetByIdAsync(int countryId);
        Task<CountryDetailsViewModel?> SetActiveAsync(SetCountryActiveViewModel model);
        Task<CountryDetailsViewModel?> UpdateAsync(UpdateCountryViewModel model);
    }
}