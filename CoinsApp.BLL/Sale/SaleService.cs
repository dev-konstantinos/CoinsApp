using CoinsApp.BLL.Sale.ViewModels;
using CoinsApp.DAL.Sale;
using CoinsApp.DAL.Sale.Models;

namespace CoinsApp.BLL.Sale;

public sealed class SaleService
{
    private readonly SaleRepository _repository;

    public SaleService(SaleRepository repository)
    {
        _repository =
            repository
            ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<SaleDetailsViewModel?> GetByIdAsync(
        int saleId)
    {
        if (saleId <= 0)
        {
            throw new ArgumentException(
                "Sale ID must be greater than zero.",
                nameof(saleId));
        }

        var sale =
            await _repository.GetByIdAsync(saleId);

        return sale is null
            ? null
            : MapToDetails(sale);
    }

    public async Task<IReadOnlyList<SaleListItemViewModel>> GetByCoinAsync(
        int coinId)
    {
        if (coinId <= 0)
        {
            throw new ArgumentException(
                "Coin ID must be greater than zero.",
                nameof(coinId));
        }

        var sales =
            await _repository.GetByCoinAsync(coinId);

        return sales
            .Select(MapToListItem)
            .ToList();
    }

    public async Task<int> CreateAsync(
        CreateSaleViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        ValidateCreate(model);

        var data = new SaleCreateData
        {
            CoinId = model.CoinId,
            SaleDate = model.SaleDate,
            SalePrice = model.SalePrice,
            CurrencyId = model.CurrencyId,
            BuyerId = model.BuyerId,
            Notes = model.Notes
        };

        return await _repository.CreateAsync(data);
    }

    public async Task<int> UpdateAsync(
        UpdateSaleViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        ValidateUpdate(model);

        var data = new SaleUpdateData
        {
            SaleId = model.SaleId,
            SaleDate = model.SaleDate,
            SalePrice = model.SalePrice,
            CurrencyId = model.CurrencyId,
            BuyerId = model.BuyerId,
            Notes = model.Notes
        };

        return await _repository.UpdateAsync(data);
    }

    public async Task<int> DeleteAsync(
        DeleteSaleViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        if (model.SaleId <= 0)
        {
            throw new ArgumentException(
                "Sale ID must be greater than zero.",
                nameof(model.SaleId));
        }

        var data = new SaleDeleteData
        {
            SaleId = model.SaleId
        };

        return await _repository.DeleteAsync(data);
    }

    private static void ValidateCreate(
        CreateSaleViewModel model)
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

        if (model.SalePrice < 0)
        {
            throw new ArgumentException(
                "Sale price cannot be negative.",
                nameof(model.SalePrice));
        }

        if (model.SaleDate > DateTime.UtcNow)
        {
            throw new ArgumentException(
                "Sale date cannot be in the future.",
                nameof(model.SaleDate));
        }

        if (model.BuyerId is <= 0)
        {
            throw new ArgumentException(
                "Buyer ID must be greater than zero when specified.",
                nameof(model.BuyerId));
        }
    }

    private static void ValidateUpdate(
        UpdateSaleViewModel model)
    {
        if (model.SaleId <= 0)
        {
            throw new ArgumentException(
                "Sale ID must be greater than zero.",
                nameof(model.SaleId));
        }

        if (model.CurrencyId <= 0)
        {
            throw new ArgumentException(
                "Currency ID must be greater than zero.",
                nameof(model.CurrencyId));
        }

        if (model.SalePrice < 0)
        {
            throw new ArgumentException(
                "Sale price cannot be negative.",
                nameof(model.SalePrice));
        }

        if (model.SaleDate > DateTime.UtcNow)
        {
            throw new ArgumentException(
                "Sale date cannot be in the future.",
                nameof(model.SaleDate));
        }

        if (model.BuyerId is <= 0)
        {
            throw new ArgumentException(
                "Buyer ID must be greater than zero when specified.",
                nameof(model.BuyerId));
        }
    }

    private static SaleListItemViewModel MapToListItem(
        SaleData sale)
    {
        return new SaleListItemViewModel
        {
            SaleId = sale.SaleId,
            CoinId = sale.CoinId,
            SaleDate = sale.SaleDate,
            SalePrice = sale.SalePrice,
            CurrencyId = sale.CurrencyId,
            CurrencyCode = sale.CurrencyCode,
            CurrencyName = sale.CurrencyName,
            PreviousOwnerId = sale.PreviousOwnerId,
            PreviousOwnerName = sale.PreviousOwnerName,
            BuyerId = sale.BuyerId,
            BuyerName = sale.BuyerName,
            Notes = sale.Notes
        };
    }

    private static SaleDetailsViewModel MapToDetails(
        SaleData sale)
    {
        return new SaleDetailsViewModel
        {
            SaleId = sale.SaleId,
            CoinId = sale.CoinId,
            SaleDate = sale.SaleDate,
            SalePrice = sale.SalePrice,
            CurrencyId = sale.CurrencyId,
            CurrencyCode = sale.CurrencyCode,
            CurrencyName = sale.CurrencyName,
            PreviousOwnerId = sale.PreviousOwnerId,
            PreviousOwnerName = sale.PreviousOwnerName,
            BuyerId = sale.BuyerId,
            BuyerName = sale.BuyerName,
            Notes = sale.Notes
        };
    }
}