using CoinsApp.BLL.PriceHistory.ViewModels;

namespace CoinsApp.BLL.PriceHistory
{
    public interface IPriceHistoryService
    {
        Task<int> CreateAsync(CreatePriceHistoryViewModel model);
        Task<int> DeleteAsync(DeletePriceHistoryViewModel model);
        Task<IReadOnlyList<PriceHistoryListItemViewModel>> GetByCoinAsync(int coinId);
        Task<int> UpdateAsync(UpdatePriceHistoryViewModel model);
    }
}