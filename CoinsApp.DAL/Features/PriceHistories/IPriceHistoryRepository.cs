using CoinsApp.DAL.Features.PriceHistories.Models;

namespace CoinsApp.DAL.Features.PriceHistories
{
    public interface IPriceHistoryRepository
    {
        Task<int> CreateAsync(PriceHistoryCreateData data);
        Task<int> DeleteAsync(PriceHistoryDeleteData data);
        Task<IReadOnlyList<PriceHistoryData>> GetByCoinAsync(int coinId);
        Task<int> UpdateAsync(PriceHistoryUpdateData data);
    }
}