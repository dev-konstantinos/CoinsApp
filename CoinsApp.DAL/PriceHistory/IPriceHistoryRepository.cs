using CoinsApp.DAL.PriceHistory.Models;

namespace CoinsApp.DAL.PriceHistory
{
    public interface IPriceHistoryRepository
    {
        Task<int> CreateAsync(PriceHistoryCreateData data);
        Task<int> DeleteAsync(PriceHistoryDeleteData data);
        Task<IReadOnlyList<PriceHistoryData>> GetByCoinAsync(int coinId);
        Task<int> UpdateAsync(PriceHistoryUpdateData data);
    }
}