namespace CoinsApp.BLL.Currencies.ViewModels;

public sealed class SetCurrencyActiveViewModel
{
    public int CurrencyId { get; init; }
    public bool IsActive { get; init; }
}