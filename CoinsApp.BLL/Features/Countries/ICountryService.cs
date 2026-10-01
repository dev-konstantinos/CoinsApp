using CoinsApp.BLL.Features.Countries.ViewModels;

namespace CoinsApp.BLL.Features.Countries
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