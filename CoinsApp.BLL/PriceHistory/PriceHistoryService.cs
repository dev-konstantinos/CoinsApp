using CoinsApp.BLL.PriceHistory.ViewModels;
using CoinsApp.DAL.PriceHistory;
using CoinsApp.DAL.PriceHistory.Models;

namespace CoinsApp.BLL.PriceHistory;

public sealed class PriceHistoryService : IPriceHistoryService
{
    private readonly IPriceHistoryRepository _repository;

    public PriceHistoryService(IPriceHistoryRepository repository)
    {
        _repository =
            repository
            ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<IReadOnlyList<PriceHistoryListItemViewModel>> GetByCoinAsync(
        int coinId)
    {
        if (coinId <= 0)
        {
            throw new ArgumentException(
                "Coin ID must be greater than zero.",
                nameof(coinId));
        }

        var entries = await _repository.GetByCoinAsync(coinId);

        return entries
            .Select(MapToListItem)
            .ToList();
    }

    public async Task<int> CreateAsync(
        CreatePriceHistoryViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        ValidateCreate(model);

        var data = new PriceHistoryCreateData
        {
            CoinId = model.CoinId,
            Price = model.Price,
            CurrencyId = model.CurrencyId,
            PriceDate = model.PriceDate,
            Source = model.Source,
            Notes = model.Notes
        };

        return await _repository.CreateAsync(data);
    }

    public async Task<int> UpdateAsync(
        UpdatePriceHistoryViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        ValidateUpdate(model);

        var data = new PriceHistoryUpdateData
        {
            PriceHistoryId = model.PriceHistoryId,
            Price = model.Price,
            CurrencyId = model.CurrencyId,
            PriceDate = model.PriceDate,
            Source = model.Source,
            Notes = model.Notes
        };

        return await _repository.UpdateAsync(data);
    }

    public async Task<int> DeleteAsync(
        DeletePriceHistoryViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        if (model.PriceHistoryId <= 0)
        {
            throw new ArgumentException(
                "Price history ID must be greater than zero.",
                nameof(model.PriceHistoryId));
        }

        var data = new PriceHistoryDeleteData
        {
            PriceHistoryId = model.PriceHistoryId
        };

        return await _repository.DeleteAsync(data);
    }

    private static void ValidateCreate(
        CreatePriceHistoryViewModel model)
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

        if (model.Price < 0)
        {
            throw new ArgumentException(
                "Price cannot be negative.",
                nameof(model.Price));
        }

        if (model.PriceDate > DateTime.UtcNow)
        {
            throw new ArgumentException(
                "Price date cannot be in the future.",
                nameof(model.PriceDate));
        }
    }

    private static void ValidateUpdate(
        UpdatePriceHistoryViewModel model)
    {
        if (model.PriceHistoryId <= 0)
        {
            throw new ArgumentException(
                "Price history ID must be greater than zero.",
                nameof(model.PriceHistoryId));
        }

        if (model.CurrencyId <= 0)
        {
            throw new ArgumentException(
                "Currency ID must be greater than zero.",
                nameof(model.CurrencyId));
        }

        if (model.Price < 0)
        {
            throw new ArgumentException(
                "Price cannot be negative.",
                nameof(model.Price));
        }

        if (model.PriceDate > DateTime.UtcNow)
        {
            throw new ArgumentException(
                "Price date cannot be in the future.",
                nameof(model.PriceDate));
        }
    }

    private static PriceHistoryListItemViewModel MapToListItem(
        PriceHistoryData data)
    {
        return new PriceHistoryListItemViewModel
        {
            PriceHistoryId = data.PriceHistoryId,
            CoinId = data.CoinId,
            Price = data.Price,
            CurrencyId = data.CurrencyId,
            CurrencyCode = data.CurrencyCode,
            CurrencyName = data.CurrencyName,
            PriceDate = data.PriceDate,
            Source = data.Source,
            Notes = data.Notes
        };
    }
}