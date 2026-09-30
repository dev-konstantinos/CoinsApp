using CoinsApp.DAL.Catalogs.Models;

namespace CoinsApp.DAL.Catalogs;

public interface ICatalogRepository
{
    Task<IReadOnlyList<CatalogData>> GetAllAsync();

    Task<CatalogData?> GetByIdAsync(
        int catalogId);

    Task<CatalogData?> CreateAsync(
        CatalogCreateData data);

    Task<CatalogData?> UpdateAsync(
        CatalogUpdateData data);

    Task<int> DeleteAsync(
        CatalogDeleteData data);
}