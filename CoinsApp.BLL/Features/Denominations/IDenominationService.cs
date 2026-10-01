using CoinsApp.BLL.Features.Denominations.ViewModels;

namespace CoinsApp.BLL.Features.Denominations
{
    public interface IDenominationService
    {
        Task<DenominationDetailsViewModel?> CreateAsync(CreateDenominationViewModel model);
        Task<IReadOnlyList<DenominationListItemViewModel>> GetAllAsync();
        Task<IReadOnlyList<DenominationListItemViewModel>> GetByCurrencyAsync(int currencyId);
        Task<DenominationDetailsViewModel?> GetByIdAsync(int denominationId);
        Task<DenominationDetailsViewModel?> SetActiveAsync(SetDenominationActiveViewModel model);
        Task<DenominationDetailsViewModel?> UpdateAsync(UpdateDenominationViewModel model);
    }
}