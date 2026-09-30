using CoinsApp.BLL.Currencies.ViewModels;

namespace CoinsApp.BLL.Currencies
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