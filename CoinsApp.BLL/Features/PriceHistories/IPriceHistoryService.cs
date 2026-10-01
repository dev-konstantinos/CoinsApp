using CoinsApp.BLL.Features.PriceHistories.ViewModels;

namespace CoinsApp.BLL.Features.PriceHistories
{
    public interface IPriceHistoryService
    {
        Task<int> CreateAsync(CreatePriceHistoryViewModel model);
        Task<int> DeleteAsync(DeletePriceHistoryViewModel model);
        Task<IReadOnlyList<PriceHistoryListItemViewModel>> GetByCoinAsync(int coinId);
        Task<int> UpdateAsync(UpdatePriceHistoryViewModel model);
    }
}