using CoinsApp.BLL.Features.Purchases.ViewModels;
using CoinsApp.DAL.Features.Purchases;
using CoinsApp.DAL.Features.Purchases.Models;

namespace CoinsApp.BLL.Features.Purchases;

public sealed class PurchaseService : IPurchaseService
{
    private readonly IPurchaseRepository _repository;

    public PurchaseService(IPurchaseRepository repository)
    {
        _repository =
            repository
            ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<PurchaseDetailsViewModel?> GetByIdAsync(
        int purchaseId)
    {
        if (purchaseId <= 0)
        {
            throw new ArgumentException(
                "Purchase ID must be greater than zero.",
                nameof(purchaseId));
        }

        var purchase =
            await _repository.GetByIdAsync(purchaseId);

        return purchase is null
            ? null
            : MapToDetails(purchase);
    }

    public async Task<IReadOnlyList<PurchaseListItemViewModel>> GetByCoinAsync(
        int coinId)
    {
        if (coinId <= 0)
        {
            throw new ArgumentException(
                "Coin ID must be greater than zero.",
                nameof(coinId));
        }

        var purchases =
            await _repository.GetByCoinAsync(coinId);

        return purchases
            .Select(MapToListItem)
            .ToList();
    }

    public async Task<int> CreateAsync(
        CreatePurchaseViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        ValidateCreate(model);

        var data = new PurchaseCreateData
        {
            CoinId = model.CoinId,
            PurchaseDate = model.PurchaseDate,
            PurchasePrice = model.PurchasePrice,
            CurrencyId = model.CurrencyId,
            SellerId = model.SellerId,
            Notes = model.Notes
        };

        return await _repository.CreateAsync(data);
    }

    public async Task<int> UpdateAsync(
        UpdatePurchaseViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        ValidateUpdate(model);

        var data = new PurchaseUpdateData
        {
            PurchaseId = model.PurchaseId,
            PurchaseDate = model.PurchaseDate,
            PurchasePrice = model.PurchasePrice,
            CurrencyId = model.CurrencyId,
            SellerId = model.SellerId,
            Notes = model.Notes
        };

        return await _repository.UpdateAsync(data);
    }

    public async Task<int> DeleteAsync(
        DeletePurchaseViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        if (model.PurchaseId <= 0)
        {
            throw new ArgumentException(
                "Purchase ID must be greater than zero.",
                nameof(model.PurchaseId));
        }

        var data = new PurchaseDeleteData
        {
            PurchaseId = model.PurchaseId
        };

        return await _repository.DeleteAsync(data);
    }

    private static void ValidateCreate(
        CreatePurchaseViewModel model)
    {
        if (model.CoinId <= 0)
        {
            throw new ArgumentException(
                "Coin ID must be greater than zero.",
                nameof(model.CoinId));
        }

        if (model.CurrencyId <= 0)
        {
            throw new ArgumentException(
                "Currency ID must be greater than zero.",
                nameof(model.CurrencyId));
        }

        if (model.PurchasePrice < 0)
        {
            throw new ArgumentException(
                "Purchase price cannot be negative.",
                nameof(model.PurchasePrice));
        }

        if (model.PurchaseDate > DateTime.UtcNow)
        {
            throw new ArgumentException(
                "Purchase date cannot be in the future.",
                nameof(model.PurchaseDate));
        }

        if (model.SellerId is <= 0)
        {
            throw new ArgumentException(
                "Seller ID must be greater than zero when specified.",
                nameof(model.SellerId));
        }
    }

    private static void ValidateUpdate(
        UpdatePurchaseViewModel model)
    {
        if (model.PurchaseId <= 0)
        {
            throw new ArgumentException(
                "Purchase ID must be greater than zero.",
                nameof(model.PurchaseId));
        }

        if (model.CurrencyId <= 0)
        {
            throw new ArgumentException(
                "Currency ID must be greater than zero.",
                nameof(model.CurrencyId));
        }

        if (model.PurchasePrice < 0)
        {
            throw new ArgumentException(
                "Purchase price cannot be negative.",
                nameof(model.PurchasePrice));
        }

        if (model.PurchaseDate > DateTime.UtcNow)
        {
            throw new ArgumentException(
                "Purchase date cannot be in the future.",
                nameof(model.PurchaseDate));
        }

        if (model.SellerId is <= 0)
        {
            throw new ArgumentException(
                "Seller ID must be greater than zero when specified.",
                nameof(model.SellerId));
        }
    }

    private static PurchaseListItemViewModel MapToListItem(
        PurchaseData purchase)
    {
        return new PurchaseListItemViewModel
        {
            PurchaseId = purchase.PurchaseId,
            CoinId = purchase.CoinId,
            PurchaseDate = purchase.PurchaseDate,
            PurchasePrice = purchase.PurchasePrice,
            CurrencyId = purchase.CurrencyId,
            CurrencyCode = purchase.CurrencyCode,
            CurrencyName = purchase.CurrencyName,
            SellerId = purchase.SellerId,
            SellerName = purchase.SellerName,
            Notes = purchase.Notes
        };
    }

    private static PurchaseDetailsViewModel MapToDetails(
        PurchaseData purchase)
    {
        return new PurchaseDetailsViewModel
        {
            PurchaseId = purchase.PurchaseId,
            CoinId = purchase.CoinId,
            PurchaseDate = purchase.PurchaseDate,
            PurchasePrice = purchase.PurchasePrice,
            CurrencyId = purchase.CurrencyId,
            CurrencyCode = purchase.CurrencyCode,
            CurrencyName = purchase.CurrencyName,
            SellerId = purchase.SellerId,
            SellerName = purchase.SellerName,
            Notes = purchase.Notes
        };
    }
}