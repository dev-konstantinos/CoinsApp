namespace CoinsApp.DAL.Purchase.Models;

public sealed class PurchaseData
{
    public int PurchaseId { get; init; }

    public int CoinId { get; init; }

    public DateTime PurchaseDate { get; init; }

    public decimal PurchasePrice { get; init; }

    public int CurrencyId { get; init; }
    public string CurrencyCode { get; init; } = string.Empty;
    public string CurrencyName { get; init; } = string.Empty;

    public int? SellerId { get; init; }
    public string? SellerName { get; init; }

    public string? Notes { get; init; }
}