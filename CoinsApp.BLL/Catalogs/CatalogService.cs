using CoinsApp.BLL.Catalogs.ViewModels;
using CoinsApp.DAL.Catalogs;
using CoinsApp.DAL.Catalogs.Models;

namespace CoinsApp.BLL.Catalogs;

public sealed class CatalogService : ICatalogService
{
    private readonly ICatalogRepository _repository;

    public CatalogService(ICatalogRepository repository)
    {
        _repository =
            repository
            ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<IReadOnlyList<CatalogListItemViewModel>> GetAllAsync()
    {
        var catalogs =
            await _repository.GetAllAsync();

        return catalogs
            .Select(MapToListItem)
            .ToList();
    }

    public async Task<CatalogDetailsViewModel?> GetByIdAsync(
        int catalogId)
    {
        if (catalogId <= 0)
        {
            throw new ArgumentException(
                "Catalog ID must be greater than zero.",
                nameof(catalogId));
        }

        var catalog =
            await _repository.GetByIdAsync(catalogId);

        return catalog is null
            ? null
            : MapToDetails(catalog);
    }

    public async Task<CatalogDetailsViewModel?> CreateAsync(
        CreateCatalogViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        ValidateCreate(model);

        var data = new CatalogCreateData
        {
            Name = model.Name.Trim(),
            ShortName = model.ShortName,
            Publisher = model.Publisher,
            Description = model.Description,
            IsActive = model.IsActive
        };

        var catalog =
            await _repository.CreateAsync(data);

        return catalog is null
            ? null
            : MapToDetails(catalog);
    }

    public async Task<CatalogDetailsViewModel?> UpdateAsync(
        UpdateCatalogViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        ValidateUpdate(model);

        var data = new CatalogUpdateData
        {
            CatalogId = model.CatalogId,
            Name = model.Name.Trim(),
            ShortName = model.ShortName,
            Publisher = model.Publisher,
            Description = model.Description,
            IsActive = model.IsActive
        };

        var catalog =
            await _repository.UpdateAsync(data);

        return catalog is null
            ? null
            : MapToDetails(catalog);
    }

    public async Task<int> DeleteAsync(
        DeleteCatalogViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        if (model.CatalogId <= 0)
        {
            throw new ArgumentException(
                "Catalog ID must be greater than zero.",
                nameof(model.CatalogId));
        }

        var data = new CatalogDeleteData
        {
            CatalogId = model.CatalogId
        };

        return await _repository.DeleteAsync(data);
    }

    private static void ValidateCreate(
        CreateCatalogViewModel model)
    {
        ValidateName(model.Name);
    }

    private static void ValidateUpdate(
        UpdateCatalogViewModel model)
    {
        if (model.CatalogId <= 0)
        {
            throw new ArgumentException(
                "Catalog ID must be greater than zero.",
                nameof(model.CatalogId));
        }

        ValidateName(model.Name);
    }

    private static void ValidateName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(
                "Catalog name is required.",
                nameof(name));
        }

        if (name.Trim().Length > 150)
        {
            throw new ArgumentException(
                "Catalog name cannot exceed 150 characters.",
                nameof(name));
        }
    }

    private static CatalogListItemViewModel MapToListItem(
        CatalogData catalog)
    {
        return new CatalogListItemViewModel
        {
            CatalogId = catalog.CatalogId,
            Name = catalog.Name,
            ShortName = catalog.ShortName,
            Publisher = catalog.Publisher,
            IsActive = catalog.IsActive
        };
    }

    private static CatalogDetailsViewModel MapToDetails(
        CatalogData catalog)
    {
        return new CatalogDetailsViewModel
        {
            CatalogId = catalog.CatalogId,
            Name = catalog.Name,
            ShortName = catalog.ShortName,
            Publisher = catalog.Publisher,
            Description = catalog.Description,
            IsActive = catalog.IsActive
        };
    }
}