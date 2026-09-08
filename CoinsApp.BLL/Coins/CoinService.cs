using CoinsApp.BLL.Coins.ViewModels;
using CoinsApp.DAL.Coins;
using CoinsApp.DAL.Coins.Models;

namespace CoinsApp.BLL.Coins;

public sealed class CoinService
{
    private readonly CoinRepository _coinRepository;

    public CoinService(CoinRepository coinRepository)
    {
        _coinRepository =
            coinRepository
            ?? throw new ArgumentNullException(nameof(coinRepository));
    }

    public async Task<IReadOnlyList<CoinListItemViewModel>> GetAllAsync()
    {
        var coins = await _coinRepository.GetAllAsync();

        return coins
            .Select(MapToListItem)
            .ToList();
    }

    public async Task<CoinDetailsViewModel?> GetByIdAsync(int coinId)
    {
        var coin = await _coinRepository.GetByIdAsync(coinId);

        return coin is null
            ? null
            : MapToDetails(coin);
    }

    public async Task<int> CreateAsync(CreateCoinViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        var data = new CoinCreateData
        {
            CollectionId = model.CollectionId,
            CountryId = model.CountryId,
            CurrencyId = model.CurrencyId,
            DenominationId = model.DenominationId,

            MintId = model.MintId,
            MaterialId = model.MaterialId,

            Year = model.Year,
            MintMark = model.MintMark,

            Fineness = model.Fineness,
            Weight = model.Weight,
            Diameter = model.Diameter,
            Thickness = model.Thickness,

            Shape = model.Shape,
            Description = model.Description,
            Designer = model.Designer,
            Mintage = model.Mintage,

            Condition = model.Condition,
            Grade = model.Grade,
            GradingCompany = model.GradingCompany,
            GradingCertificateNumber =
                model.GradingCertificateNumber,


            Notes = model.Notes
        };

        return await _coinRepository.CreateAsync(data);
    }

    private static CoinListItemViewModel MapToListItem(CoinData coin)
    {
        return new CoinListItemViewModel
        {
            CoinId = coin.CoinId,
            Country = coin.CountryName,
            Denomination = coin.DenominationDisplayName,
            Currency = coin.CurrencyCode,
            Year = coin.Year,
            Mint = coin.MintName,
            OwnerName = coin.OwnerName,
            CurrentPrice = coin.CurrentPrice,
            CurrentPriceCurrency =
                coin.CurrentPriceCurrencyCode
        };
    }

    private static CoinDetailsViewModel MapToDetails(CoinData coin)
    {
        return new CoinDetailsViewModel
        {
            CoinId = coin.CoinId,
            CollectionId = coin.CollectionId,

            OwnerId = coin.OwnerId,
            OwnerName = coin.OwnerName,

            CountryId = coin.CountryId,
            CountryName = coin.CountryName,

            CurrencyId = coin.CurrencyId,
            CurrencyCode = coin.CurrencyCode,
            CurrencyName = coin.CurrencyName,

            DenominationId = coin.DenominationId,
            DenominationDisplayName =
                coin.DenominationDisplayName,

            MintId = coin.MintId,
            MintName = coin.MintName,

            MaterialId = coin.MaterialId,
            MaterialName = coin.MaterialName,

            Year = coin.Year,
            MintMark = coin.MintMark,

            Fineness = coin.Fineness,
            Weight = coin.Weight,
            Diameter = coin.Diameter,
            Thickness = coin.Thickness,

            Shape = coin.Shape,
            Description = coin.Description,
            Designer = coin.Designer,
            Mintage = coin.Mintage,

            Condition = coin.Condition,
            Grade = coin.Grade,

            GradingCompany = coin.GradingCompany,
            GradingCertificateNumber =
                coin.GradingCertificateNumber,

            CurrentPrice = coin.CurrentPrice,
            CurrentPriceCurrencyId =
                coin.CurrentPriceCurrencyId,
            CurrentPriceCurrencyCode =
                coin.CurrentPriceCurrencyCode,
            CurrentPriceDate =
                coin.CurrentPriceDate,

            Notes = coin.Notes,
            CreatedAt = coin.CreatedAt
        };
    }

    public async Task<int> UpdateAsync(UpdateCoinViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        if (model.CoinId <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(model.CoinId),
                "Coin ID must be greater than zero.");
        }

        var data = new CoinUpdateData
        {
            CoinId = model.CoinId,

            CollectionId = model.CollectionId,
            CountryId = model.CountryId,
            CurrencyId = model.CurrencyId,
            DenominationId = model.DenominationId,

            MintId = model.MintId,
            MaterialId = model.MaterialId,

            Year = model.Year,
            MintMark = model.MintMark,

            Fineness = model.Fineness,
            Weight = model.Weight,
            Diameter = model.Diameter,
            Thickness = model.Thickness,

            Shape = model.Shape,
            Description = model.Description,
            Designer = model.Designer,
            Mintage = model.Mintage,

            Condition = model.Condition,
            Grade = model.Grade,
            GradingCompany = model.GradingCompany,
            GradingCertificateNumber =
                model.GradingCertificateNumber,


            Notes = model.Notes
        };

        return await _coinRepository.UpdateAsync(data);
    }

    public async Task<int> DeleteAsync(DeleteCoinViewModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        if (model.CoinId <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(model.CoinId),
                "Coin ID must be greater than zero.");
        }

        var data = new CoinDeleteData
        {
            CoinId = model.CoinId
        };

        return await _coinRepository.DeleteAsync(data);
    }
}