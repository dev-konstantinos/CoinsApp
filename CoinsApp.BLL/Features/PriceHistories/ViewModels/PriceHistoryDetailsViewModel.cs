namespace CoinsApp.BLL.Features.PriceHistories.ViewModels;

public sealed class PriceHistoryDetailsViewModel
{
    public int PriceHistoryId { get; init; }

    public int CoinId { get; init; }

    public decimal Price { get; init; }

    public int CurrencyId { get; init; }

    public string CurrencyCode { get; init; } = string.Empty;

    public string CurrencyName { get; init; } = string.Empty;

    public DateTime PriceDate { get; init; }

    public string? Source { get; init; }

    public string? Notes { get; init; }
}