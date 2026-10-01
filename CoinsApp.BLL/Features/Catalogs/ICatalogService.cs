using CoinsApp.BLL.Features.Catalogs.ViewModels;

namespace CoinsApp.BLL.Features.Catalogs;

public interface ICatalogService
{
    Task<IReadOnlyList<CatalogListItemViewModel>> GetAllAsync();

    Task<CatalogDetailsViewModel?> GetByIdAsync(
        int catalogId);

    Task<CatalogDetailsViewModel?> CreateAsync(
        CreateCatalogViewModel model);

    Task<CatalogDetailsViewModel?> UpdateAsync(
        UpdateCatalogViewModel model);

    Task<int> DeleteAsync(
        DeleteCatalogViewModel model);
}