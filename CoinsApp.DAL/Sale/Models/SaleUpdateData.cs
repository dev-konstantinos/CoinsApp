namespace CoinsApp.DAL.Sale.Models;

public sealed class SaleUpdateData
{
    public int SaleId { get; init; }

    public DateTime SaleDate { get; init; }

    public decimal SalePrice { get; init; }

    public int CurrencyId { get; init; }

    public int BuyerId { get; init; }

    public string? Notes { get; init; }
}