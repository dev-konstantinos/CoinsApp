using CoinsApp.BLL.Purchase.ViewModels;

namespace CoinsApp.BLL.Purchase
{
    public interface IPurchaseService
    {
        Task<int> CreateAsync(CreatePurchaseViewModel model);
        Task<int> DeleteAsync(DeletePurchaseViewModel model);
        Task<IReadOnlyList<PurchaseListItemViewModel>> GetByCoinAsync(int coinId);
        Task<PurchaseDetailsViewModel?> GetByIdAsync(int purchaseId);
        Task<int> UpdateAsync(UpdatePurchaseViewModel model);
    }
}