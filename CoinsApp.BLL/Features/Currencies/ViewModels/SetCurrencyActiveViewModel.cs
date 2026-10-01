namespace CoinsApp.BLL.Features.Currencies.ViewModels;

public sealed class SetCurrencyActiveViewModel
{
    public int CurrencyId { get; init; }
    public bool IsActive { get; init; }
}