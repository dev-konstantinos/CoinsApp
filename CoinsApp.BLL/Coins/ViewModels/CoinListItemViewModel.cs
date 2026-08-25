namespace CoinsApp.BLL.Coins.ViewModels;

public sealed class CoinListItemViewModel
{
    public int CoinId { get; init; }

    public string Country { get; init; } = string.Empty;

    public string Denomination { get; init; } = string.Empty;

    public string Currency { get; init; } = string.Empty;

    public short? Year { get; init; }

    public string? Mint { get; init; }

    public decimal? CurrentPrice { get; init; }

    public string? CurrentPriceCurrency { get; init; }
}