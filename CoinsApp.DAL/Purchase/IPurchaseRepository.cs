using CoinsApp.DAL.Purchase.Models;

namespace CoinsApp.DAL.Purchase
{
    public interface IPurchaseRepository
    {
        Task<int> CreateAsync(PurchaseCreateData data);
        Task<int> DeleteAsync(PurchaseDeleteData data);
        Task<IReadOnlyList<PurchaseData>> GetByCoinAsync(int coinId);
        Task<PurchaseData?> GetByIdAsync(int purchaseId);
        Task<int> UpdateAsync(PurchaseUpdateData data);
    }
}