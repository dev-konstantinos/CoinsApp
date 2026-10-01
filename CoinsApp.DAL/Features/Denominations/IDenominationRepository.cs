using CoinsApp.DAL.Features.Denominations.Models;

namespace CoinsApp.DAL.Features.Denominations
{
    public interface IDenominationRepository
    {
        Task<DenominationData?> CreateAsync(DenominationCreateData data);
        Task<IReadOnlyList<DenominationData>> GetAllAsync();
        Task<IReadOnlyList<DenominationData>> GetByCurrencyAsync(int currencyId);
        Task<DenominationData?> GetByIdAsync(int denominationId);
        Task<DenominationData?> SetActiveAsync(DenominationSetActiveData data);
        Task<DenominationData?> UpdateAsync(DenominationUpdateData data);
    }
}