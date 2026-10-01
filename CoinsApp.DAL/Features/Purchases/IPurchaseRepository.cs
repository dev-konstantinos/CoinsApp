using CoinsApp.DAL.Features.Purchases.Models;

namespace CoinsApp.DAL.Features.Purchases
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