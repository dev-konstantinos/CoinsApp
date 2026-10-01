using CoinsApp.BLL.Features.CatalogEntries.ViewModels;
using CoinsApp.DAL.Features.CatalogEntries;
using CoinsApp.DAL.Features.CatalogEntries.Models;

namespace CoinsApp.BLL.Features.CatalogEntries;

public sealed class CatalogEntryService : ICatalogEntryService
{
    private readonly ICatalogEntryRepository _repository;

    public CatalogEntryService(ICatalogEntryRepository repository)
    {
        _repository =
            repository
            ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<IReadOnlyList<CatalogEntryListItemViewModel>> GetAllAsync()
    {
        var entries =
            await _repository.GetAllAsync();

        return entries
            .Select(MapToListItem)
            .ToList();
    }

    public async Task<CatalogEntryDetailsViewModel?> GetByIdAsync(
        int catalogEntryId)
    {
        if (catalogEntryId <= 0)
        {
            throw new ArgumentException(
                "Catalog entry ID must be greater than zero.",
                nameof(catalogEntryId));
        }

        var entry =
            await _repository.GetByIdAsync(catalogEntryId);

        return entry is null
            ? null
            : MapToDetails(entry);
    }

    public async Task<IReadOnlyList<CatalogEntryListItemViewModel>> GetByCoinAsync(
        int coinId)
    {
        if (coinId <= 0)
        {
            throw new ArgumentException(
                "Coin ID must be greater than zero.",
                nameof(coinId));
        }

        var entries =
            await _repository.GetByCoinAsync(coinId);

        return entries
            .Select(MapToListItem)
            .ToList();
    }

    public async Task<IReadOnlyList<CatalogEntryListItemViewModel>> GetByCatalogAsync(
        int catalogId)
    {
        if (catalogId <= 0)
        {
            throw new ArgumentException(
                "Catalog ID must be greater than zero.",
                nameof(catalogId));
        }

        var entries =
            await _repository.GetByCatalogAsync(catalogId);

        return entries
            .Select(MapToListItem)
            .ToList();
    }

    public async Task<CatalogEntryDetailsViewModel?> CreateAsync(
        CreateCatalogEntryViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        ValidateCreate(model);

        var data = new CatalogEntryCreateData
        {
            CatalogId = model.CatalogId,
            CoinId = model.CoinId,
            CatalogNumber = model.CatalogNumber.Trim(),
            Notes = model.Notes
        };

        var entry =
            await _repository.CreateAsync(data);

        return entry is null
            ? null
            : MapToDetails(entry);
    }

    public async Task<CatalogEntryDetailsViewModel?> UpdateAsync(
        UpdateCatalogEntryViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        ValidateUpdate(model);

        var data = new CatalogEntryUpdateData
        {
            CatalogEntryId = model.CatalogEntryId,
            CatalogId = model.CatalogId,
            CoinId = model.CoinId,
            CatalogNumber = model.CatalogNumber.Trim(),
            Notes = model.Notes
        };

        var entry =
            await _repository.UpdateAsync(data);

        return entry is null
            ? null
            : MapToDetails(entry);
    }

    public async Task<int> DeleteAsync(
        DeleteCatalogEntryViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        if (model.CatalogEntryId <= 0)
        {
            throw new ArgumentException(
                "Catalog entry ID must be greater than zero.",
                nameof(model.CatalogEntryId));
        }

        var data = new CatalogEntryDeleteData
        {
            CatalogEntryId = model.CatalogEntryId
        };

        return await _repository.DeleteAsync(data);
    }

    private static void ValidateCreate(
        CreateCatalogEntryViewModel model)
    {
        ValidateCatalogId(model.CatalogId);
        ValidateCoinId(model.CoinId);
        ValidateCatalogNumber(model.CatalogNumber);
    }

    private static void ValidateUpdate(
        UpdateCatalogEntryViewModel model)
    {
        if (model.CatalogEntryId <= 0)
        {
            throw new ArgumentException(
                "Catalog entry ID must be greater than zero.",
                nameof(model.CatalogEntryId));
        }

        ValidateCatalogId(model.CatalogId);
        ValidateCoinId(model.CoinId);
        ValidateCatalogNumber(model.CatalogNumber);
    }

    private static void ValidateCatalogId(int catalogId)
    {
        if (catalogId <= 0)
        {
            throw new ArgumentException(
                "Catalog ID must be greater than zero.",
                nameof(catalogId));
        }
    }

    private static void ValidateCoinId(int coinId)
    {
        if (coinId <= 0)
        {
            throw new ArgumentException(
                "Coin ID must be greater than zero.",
                nameof(coinId));
        }
    }

    private static void ValidateCatalogNumber(string? catalogNumber)
    {
        if (string.IsNullOrWhiteSpace(catalogNumber))
        {
            throw new ArgumentException(
                "Catalog number is required.",
                nameof(catalogNumber));
        }

        if (catalogNumber.Trim().Length > 50)
        {
            throw new ArgumentException(
                "Catalog number cannot exceed 50 characters.",
                nameof(catalogNumber));
        }
    }

    private static CatalogEntryListItemViewModel MapToListItem(
        CatalogEntryData entry)
    {
        return new CatalogEntryListItemViewModel
        {
            CatalogEntryId = entry.CatalogEntryId,
            CatalogId = entry.CatalogId,
            CoinId = entry.CoinId,
            CatalogNumber = entry.CatalogNumber,
            Notes = entry.Notes
        };
    }

    private static CatalogEntryDetailsViewModel MapToDetails(
        CatalogEntryData entry)
    {
        return new CatalogEntryDetailsViewModel
        {
            CatalogEntryId = entry.CatalogEntryId,
            CatalogId = entry.CatalogId,
            CoinId = entry.CoinId,
            CatalogNumber = entry.CatalogNumber,
            Notes = entry.Notes
        };
    }
}