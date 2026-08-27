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

    public IReadOnlyList<CoinListItemViewModel> GetAll()
    {
        return _coinRepository
            .GetAll()
            .Select(MapToListItem)
            .ToList();
    }

    public CoinDetailsViewModel? GetById(int coinId)
    {
        var coin = _coinRepository.GetById(coinId);

        return coin is null
            ? null
            : MapToDetails(coin);
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
            CurrentPrice = coin.CurrentPrice,
            CurrentPriceCurrency = coin.CurrentPriceCurrencyCode
        };
    }

    private static CoinDetailsViewModel MapToDetails(CoinData coin)
    {
        return new CoinDetailsViewModel
        {
            CoinId = coin.CoinId,
            CollectionId = coin.CollectionId,

            CountryId = coin.CountryId,
            CountryName = coin.CountryName,

            CurrencyId = coin.CurrencyId,
            CurrencyCode = coin.CurrencyCode,
            CurrencyName = coin.CurrencyName,

            DenominationId = coin.DenominationId,
            DenominationDisplayName = coin.DenominationDisplayName,

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
            GradingCertificateNumber = coin.GradingCertificateNumber,

            CurrentPrice = coin.CurrentPrice,
            CurrentPriceCurrencyId = coin.CurrentPriceCurrencyId,
            CurrentPriceCurrencyCode = coin.CurrentPriceCurrencyCode,
            CurrentPriceDate = coin.CurrentPriceDate,

            Notes = coin.Notes,
            CreatedAt = coin.CreatedAt
        };
    }
}