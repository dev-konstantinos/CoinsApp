using CoinsApp.BLL.Coins.ViewModels;
using CoinsApp.DAL.Coins;
using CoinsApp.DAL.Coins.Models;

namespace CoinsApp.BLL.Coins;

public sealed class CoinService
{
    private readonly CoinRepository _coinRepository;

    public CoinService(CoinRepository coinRepository)
    {
        _coinRepository = coinRepository ?? throw new ArgumentNullException(nameof(coinRepository));
    }

    public IReadOnlyList<CoinListItemViewModel> GetAll()
    {
        var coins = _coinRepository.GetAll();

        return coins
            .Select(MapToListItem)
            .ToList();
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
}