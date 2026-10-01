namespace CoinsApp.BLL.Features.PriceHistories.ViewModels;

public sealed class CreatePriceHistoryViewModel
{
    public int CoinId { get; init; }

    public decimal Price { get; init; }

    public int CurrencyId { get; init; }

    public DateTime PriceDate { get; init; }

    public string? Source { get; init; }

    public string? Notes { get; init; }
}