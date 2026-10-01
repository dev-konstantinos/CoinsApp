using CoinsApp.BLL.Features.Coins.ViewModels;

namespace CoinsApp.BLL.Features.Coins
{
    public interface ICoinService
    {
        Task<int> CreateAsync(CreateCoinViewModel model);
        Task<int> DeleteAsync(DeleteCoinViewModel model);
        Task<IReadOnlyList<CoinListItemViewModel>> GetAllAsync();
        Task<CoinDetailsViewModel?> GetByIdAsync(int coinId);
        Task<int> UpdateAsync(UpdateCoinViewModel model);
    }
}