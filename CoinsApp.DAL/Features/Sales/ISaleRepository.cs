using CoinsApp.DAL.Features.Sales.Models;

namespace CoinsApp.DAL.Features.Sales
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