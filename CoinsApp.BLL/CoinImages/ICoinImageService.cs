using CoinsApp.BLL.CoinImages.ViewModels;

namespace CoinsApp.BLL.CoinImages;

public interface ICoinImageService
{
    Task<IReadOnlyList<CoinImageListItemViewModel>> GetAllAsync();

    Task<CoinImageDetailsViewModel?> GetByIdAsync(
        int coinImageId);

    Task<IReadOnlyList<CoinImageListItemViewModel>> GetByCoinAsync(
        int coinId);

    Task<CoinImageDetailsViewModel?> CreateAsync(
        CreateCoinImageViewModel model);

    Task<CoinImageDetailsViewModel?> UpdateAsync(
        UpdateCoinImageViewModel model);

    Task<int> DeleteAsync(
        DeleteCoinImageViewModel model);
}