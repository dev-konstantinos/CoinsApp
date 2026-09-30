using CoinsApp.DAL.Sale.Models;

namespace CoinsApp.DAL.Sale
{
    public interface ISaleRepository
    {
        Task<int> CreateAsync(SaleCreateData data);
        Task<int> DeleteAsync(SaleDeleteData data);
        Task<IReadOnlyList<SaleData>> GetByCoinAsync(int coinId);
        Task<SaleData?> GetByIdAsync(int saleId);
        Task<int> UpdateAsync(SaleUpdateData data);
    }
}