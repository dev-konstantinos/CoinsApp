namespace CoinsApp.BLL.Sale.ViewModels;

public sealed class SaleViewModel
{
    public int SaleId { get; init; }

    public int CoinId { get; init; }

    public DateTime SaleDate { get; init; }

    public decimal SalePrice { get; init; }

    public int CurrencyId { get; init; }

    public string CurrencyCode { get; init; } = string.Empty;

    public string CurrencyName { get; init; } = string.Empty;

    public int? BuyerId { get; init; }

    public string? BuyerName { get; init; }

    public string? Notes { get; init; }
}