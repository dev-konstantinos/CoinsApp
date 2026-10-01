namespace CoinsApp.BLL.Features.Purchases.ViewModels;

public sealed class CreatePurchaseViewModel
{
    public int CoinId { get; init; }

    public DateTime PurchaseDate { get; init; }

    public decimal PurchasePrice { get; init; }

    public int CurrencyId { get; init; }

    public int? SellerId { get; init; }

    public string? Notes { get; init; }
}