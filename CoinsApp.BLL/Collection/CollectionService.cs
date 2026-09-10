using CoinsApp.BLL.Collection.ViewModels;
using CoinsApp.DAL.Collection;
using CoinsApp.DAL.Collection.Models;

namespace CoinsApp.BLL.Collection;

public sealed class CollectionService
{
    private readonly CollectionRepository _repository;

    public CollectionService(CollectionRepository repository)
    {
        _repository =
            repository
            ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<IReadOnlyList<CollectionListItemViewModel>> GetAllAsync(
        int? userId = null)
    {
        if (userId is <= 0)
        {
            throw new ArgumentException(
                "User ID must be greater than zero when specified.",
                nameof(userId));
        }

        var collections =
            await _repository.GetAllAsync(userId);

        return collections
            .Select(MapToListItem)
            .ToList();
    }

    public async Task<CollectionDetailsViewModel?> GetByIdAsync(
        int collectionId)
    {
        if (collectionId <= 0)
        {
            throw new ArgumentException(
                "Collection ID must be greater than zero.",
                nameof(collectionId));
        }

        var collection =
            await _repository.GetByIdAsync(collectionId);

        return collection is null
            ? null
            : MapToDetails(collection);
    }

    public async Task<int> CreateAsync(
        CreateCollectionViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        ValidateCreate(model);

        var data = new CollectionCreateData
        {
            UserId = model.UserId,
            Name = model.Name.Trim(),
            Description = model.Description,
            IsActive = model.IsActive
        };

        return await _repository.CreateAsync(data);
    }

    public async Task<CollectionDetailsViewModel?> UpdateAsync(
        UpdateCollectionViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        ValidateUpdate(model);

        var data = new CollectionUpdateData
        {
            CollectionId = model.CollectionId,
            Name = model.Name.Trim(),
            Description = model.Description,
            IsActive = model.IsActive
        };

        var collection =
            await _repository.UpdateAsync(data);

        return collection is null
            ? null
            : MapToDetails(collection);
    }

    public async Task<int> DeleteAsync(
        DeleteCollectionViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        if (model.CollectionId <= 0)
        {
            throw new ArgumentException(
                "Collection ID must be greater than zero.",
                nameof(model.CollectionId));
        }

        var data = new CollectionDeleteData
        {
            CollectionId = model.CollectionId
        };

        return await _repository.DeleteAsync(data);
    }

    private static void ValidateCreate(
        CreateCollectionViewModel model)
    {
        if (model.UserId <= 0)
        {
            throw new ArgumentException(
                "User ID must be greater than zero.",
                nameof(model.UserId));
        }

        ValidateName(model.Name);
    }

    private static void ValidateUpdate(
        UpdateCollectionViewModel model)
    {
        if (model.CollectionId <= 0)
        {
            throw new ArgumentException(
                "Collection ID must be greater than zero.",
                nameof(model.CollectionId));
        }

        ValidateName(model.Name);
    }

    private static void ValidateName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(
                "Collection name is required.",
                nameof(name));
        }

        if (name.Trim().Length > 150)
        {
            throw new ArgumentException(
                "Collection name cannot exceed 150 characters.",
                nameof(name));
        }
    }

    private static CollectionListItemViewModel MapToListItem(
        CollectionData collection)
    {
        return new CollectionListItemViewModel
        {
            CollectionId = collection.CollectionId,
            UserId = collection.UserId,
            UserName = collection.UserName,
            Name = collection.Name,
            Description = collection.Description,
            IsActive = collection.IsActive,
            CreatedAt = collection.CreatedAt,
            CoinCount = collection.CoinCount
        };
    }

    private static CollectionDetailsViewModel MapToDetails(
        CollectionData collection)
    {
        return new CollectionDetailsViewModel
        {
            CollectionId = collection.CollectionId,
            UserId = collection.UserId,
            UserName = collection.UserName,
            Name = collection.Name,
            Description = collection.Description,
            IsActive = collection.IsActive,
            CreatedAt = collection.CreatedAt,
            CoinCount = collection.CoinCount
        };
    }
}