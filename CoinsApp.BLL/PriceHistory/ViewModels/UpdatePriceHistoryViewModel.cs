namespace CoinsApp.BLL.PriceHistory.ViewModels;

public sealed class UpdatePriceHistoryViewModel
{
    public int PriceHistoryId { get; init; }

    public decimal Price { get; init; }

    public int CurrencyId { get; init; }

    public DateTime PriceDate { get; init; }

    public string? Source { get; init; }

    public string? Notes { get; init; }
}