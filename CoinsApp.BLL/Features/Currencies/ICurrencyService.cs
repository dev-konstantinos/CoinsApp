using CoinsApp.BLL.Features.Currencies.ViewModels;

namespace CoinsApp.BLL.Features.Currencies
{
    public interface ICurrencyService
    {
        Task<CurrencyDetailsViewModel?> CreateAsync(CreateCurrencyViewModel model);
        Task<IReadOnlyList<CurrencyListItemViewModel>> GetAllAsync();
        Task<CurrencyDetailsViewModel?> GetByIdAsync(int currencyId);
        Task<CurrencyDetailsViewModel?> SetActiveAsync(SetCurrencyActiveViewModel model);
        Task<CurrencyDetailsViewModel?> UpdateAsync(UpdateCurrencyViewModel model);
    }
}