using CoinsApp.BLL.Features.Mints.ViewModels;

namespace CoinsApp.BLL.Features.Mints
{
    public interface IMintService
    {
        Task<MintDetailsViewModel?> CreateAsync(CreateMintViewModel model);
        Task<IReadOnlyList<MintListItemViewModel>> GetAllAsync();
        Task<IReadOnlyList<MintListItemViewModel>> GetByCountryAsync(int countryId);
        Task<MintDetailsViewModel?> GetByIdAsync(int mintId);
        Task<MintDetailsViewModel?> SetActiveAsync(SetMintActiveViewModel model);
        Task<MintDetailsViewModel?> UpdateAsync(UpdateMintViewModel model);
    }
}