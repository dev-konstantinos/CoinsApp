using CoinsApp.DAL.Features.Coins.Models;

namespace CoinsApp.DAL.Features.Coins
{
    public interface ICoinRepository
    {
        Task<int> CreateAsync(CoinCreateData data);
        Task<int> DeleteAsync(CoinDeleteData data);
        Task<IReadOnlyList<CoinData>> GetAllAsync();
        Task<CoinData?> GetByIdAsync(int coinId);
        Task<int> UpdateAsync(CoinUpdateData data);
    }
}