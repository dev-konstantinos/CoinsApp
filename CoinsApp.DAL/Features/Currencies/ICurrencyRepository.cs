using CoinsApp.DAL.Features.Currencies.Models;

namespace CoinsApp.DAL.Features.Currencies
{
    public interface ICurrencyRepository
    {
        Task<CurrencyData?> CreateAsync(CurrencyCreateData data);
        Task<IReadOnlyList<CurrencyData>> GetAllAsync();
        Task<CurrencyData?> GetByIdAsync(int currencyId);
        Task<CurrencyData?> SetActiveAsync(CurrencySetActiveData data);
        Task<CurrencyData?> UpdateAsync(CurrencyUpdateData data);
    }
}