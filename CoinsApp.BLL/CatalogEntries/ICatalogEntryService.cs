using CoinsApp.BLL.CatalogEntries.ViewModels;

namespace CoinsApp.BLL.CatalogEntries;

public interface ICatalogEntryService
{
    Task<IReadOnlyList<CatalogEntryListItemViewModel>> GetAllAsync();

    Task<CatalogEntryDetailsViewModel?> GetByIdAsync(
        int catalogEntryId);

    Task<IReadOnlyList<CatalogEntryListItemViewModel>> GetByCoinAsync(
        int coinId);

    Task<IReadOnlyList<CatalogEntryListItemViewModel>> GetByCatalogAsync(
        int catalogId);

    Task<CatalogEntryDetailsViewModel?> CreateAsync(
        CreateCatalogEntryViewModel model);

    Task<CatalogEntryDetailsViewModel?> UpdateAsync(
        UpdateCatalogEntryViewModel model);

    Task<int> DeleteAsync(
        DeleteCatalogEntryViewModel model);
}