namespace CoinsApp.BLL.CountryCurrencies.ViewModels;

public sealed class SetCountryCurrencyActiveViewModel
{
    public int CountryCurrencyId { get; init; }

    public bool IsActive { get; init; }
}