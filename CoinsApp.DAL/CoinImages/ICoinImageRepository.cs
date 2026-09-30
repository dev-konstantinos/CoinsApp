using CoinsApp.DAL.CoinImages.Models;

namespace CoinsApp.DAL.CoinImages
{
    public interface ICoinImageRepository
    {
        Task<CoinImageData?> CreateAsync(CoinImageCreateData data);
        Task<int> DeleteAsync(CoinImageDeleteData data);
        Task<IReadOnlyList<CoinImageData>> GetAllAsync();
        Task<IReadOnlyList<CoinImageData>> GetByCoinAsync(int coinId);
        Task<CoinImageData?> GetByIdAsync(int coinImageId);
        Task<CoinImageData?> UpdateAsync(CoinImageUpdateData data);
    }
}