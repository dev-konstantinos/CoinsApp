namespace CoinsApp.BLL.Sale.ViewModels;

public sealed class CreateSaleViewModel
{
    public int CoinId { get; init; }

    public DateTime SaleDate { get; init; }

    public decimal SalePrice { get; init; }

    public int CurrencyId { get; init; }

    public int BuyerId { get; init; }

    public string? Notes { get; init; }
}