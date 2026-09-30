using CoinsApp.DAL.CatalogEntries.Models;

namespace CoinsApp.DAL.CatalogEntries;

public interface ICatalogEntryRepository
{
    Task<IReadOnlyList<CatalogEntryData>> GetAllAsync();

    Task<CatalogEntryData?> GetByIdAsync(
        int catalogEntryId);

    Task<IReadOnlyList<CatalogEntryData>> GetByCoinAsync(
        int coinId);

    Task<IReadOnlyList<CatalogEntryData>> GetByCatalogAsync(
        int catalogId);

    Task<CatalogEntryData?> CreateAsync(
        CatalogEntryCreateData data);

    Task<CatalogEntryData?> UpdateAsync(
        CatalogEntryUpdateData data);

    Task<int> DeleteAsync(
        CatalogEntryDeleteData data);
}