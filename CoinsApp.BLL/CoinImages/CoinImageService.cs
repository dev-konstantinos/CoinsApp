using CoinsApp.BLL.CoinImages.ViewModels;
using CoinsApp.DAL.CoinImages;
using CoinsApp.DAL.CoinImages.Models;

namespace CoinsApp.BLL.CoinImages;

public sealed class CoinImageService : ICoinImageService
{
    private readonly ICoinImageRepository _repository;

    public CoinImageService(ICoinImageRepository repository)
    {
        _repository =
            repository
            ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<IReadOnlyList<CoinImageListItemViewModel>> GetAllAsync()
    {
        var images =
            await _repository.GetAllAsync();

        return images
            .Select(MapToListItem)
            .ToList();
    }

    public async Task<CoinImageDetailsViewModel?> GetByIdAsync(
        int coinImageId)
    {
        if (coinImageId <= 0)
        {
            throw new ArgumentException(
                "Coin image ID must be greater than zero.",
                nameof(coinImageId));
        }

        var image =
            await _repository.GetByIdAsync(coinImageId);

        return image is null
            ? null
            : MapToDetails(image);
    }

    public async Task<IReadOnlyList<CoinImageListItemViewModel>> GetByCoinAsync(
        int coinId)
    {
        if (coinId <= 0)
        {
            throw new ArgumentException(
                "Coin ID must be greater than zero.",
                nameof(coinId));
        }

        var images =
            await _repository.GetByCoinAsync(coinId);

        return images
            .Select(MapToListItem)
            .ToList();
    }

    public async Task<CoinImageDetailsViewModel?> CreateAsync(
        CreateCoinImageViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        ValidateCreate(model);

        var data = new CoinImageCreateData
        {
            CoinId = model.CoinId,
            ImageType = model.ImageType.Trim(),
            FileName = model.FileName.Trim(),
            FilePath = model.FilePath.Trim(),
            Description = model.Description,
            SortOrder = model.SortOrder
        };

        var image =
            await _repository.CreateAsync(data);

        return image is null
            ? null
            : MapToDetails(image);
    }

    public async Task<CoinImageDetailsViewModel?> UpdateAsync(
        UpdateCoinImageViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        ValidateUpdate(model);

        var data = new CoinImageUpdateData
        {
            CoinImageId = model.CoinImageId,
            CoinId = model.CoinId,
            ImageType = model.ImageType.Trim(),
            FileName = model.FileName.Trim(),
            FilePath = model.FilePath.Trim(),
            Description = model.Description,
            SortOrder = model.SortOrder
        };

        var image =
            await _repository.UpdateAsync(data);

        return image is null
            ? null
            : MapToDetails(image);
    }

    public async Task<int> DeleteAsync(
        DeleteCoinImageViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        if (model.CoinImageId <= 0)
        {
            throw new ArgumentException(
                "Coin image ID must be greater than zero.",
                nameof(model.CoinImageId));
        }

        var data = new CoinImageDeleteData
        {
            CoinImageId = model.CoinImageId
        };

        return await _repository.DeleteAsync(data);
    }

    private static void ValidateCreate(
        CreateCoinImageViewModel model)
    {
        ValidateCoinId(model.CoinId);
        ValidateImageType(model.ImageType);
        ValidateFileName(model.FileName);
        ValidateFilePath(model.FilePath);
        ValidateDescription(model.Description);
        ValidateSortOrder(model.SortOrder);
    }

    private static void ValidateUpdate(
        UpdateCoinImageViewModel model)
    {
        if (model.CoinImageId <= 0)
        {
            throw new ArgumentException(
                "Coin image ID must be greater than zero.",
                nameof(model.CoinImageId));
        }

        ValidateCoinId(model.CoinId);
        ValidateImageType(model.ImageType);
        ValidateFileName(model.FileName);
        ValidateFilePath(model.FilePath);
        ValidateDescription(model.Description);
        ValidateSortOrder(model.SortOrder);
    }

    private static void ValidateCoinId(
        int coinId)
    {
        if (coinId <= 0)
        {
            throw new ArgumentException(
                "Coin ID must be greater than zero.",
                nameof(coinId));
        }
    }

    private static void ValidateImageType(
        string? imageType)
    {
        if (string.IsNullOrWhiteSpace(imageType))
        {
            throw new ArgumentException(
                "Image type is required.",
                nameof(imageType));
        }

        if (imageType.Trim().Length > 30)
        {
            throw new ArgumentException(
                "Image type cannot exceed 30 characters.",
                nameof(imageType));
        }
    }

    private static void ValidateFileName(
        string? fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentException(
                "File name is required.",
                nameof(fileName));
        }

        if (fileName.Trim().Length > 255)
        {
            throw new ArgumentException(
                "File name cannot exceed 255 characters.",
                nameof(fileName));
        }
    }

    private static void ValidateFilePath(
        string? filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException(
                "File path is required.",
                nameof(filePath));
        }

        if (filePath.Trim().Length > 1000)
        {
            throw new ArgumentException(
                "File path cannot exceed 1000 characters.",
                nameof(filePath));
        }
    }

    private static void ValidateDescription(
        string? description)
    {
        if (description is not null &&
            description.Trim().Length > 500)
        {
            throw new ArgumentException(
                "Description cannot exceed 500 characters.",
                nameof(description));
        }
    }

    private static void ValidateSortOrder(
        int sortOrder)
    {
        if (sortOrder < 0)
        {
            throw new ArgumentException(
                "Sort order cannot be negative.",
                nameof(sortOrder));
        }
    }

    private static CoinImageListItemViewModel MapToListItem(
        CoinImageData image)
    {
        return new CoinImageListItemViewModel
        {
            CoinImageId = image.CoinImageId,
            CoinId = image.CoinId,
            ImageType = image.ImageType,
            FileName = image.FileName,
            FilePath = image.FilePath,
            Description = image.Description,
            SortOrder = image.SortOrder,
            CreatedAt = image.CreatedAt
        };
    }

    private static CoinImageDetailsViewModel MapToDetails(
        CoinImageData image)
    {
        return new CoinImageDetailsViewModel
        {
            CoinImageId = image.CoinImageId,
            CoinId = image.CoinId,
            ImageType = image.ImageType,
            FileName = image.FileName,
            FilePath = image.FilePath,
            Description = image.Description,
            SortOrder = image.SortOrder,
            CreatedAt = image.CreatedAt
        };
    }
}