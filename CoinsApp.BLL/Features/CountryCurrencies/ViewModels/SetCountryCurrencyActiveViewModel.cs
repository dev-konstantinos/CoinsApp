namespace CoinsApp.BLL.Features.CountryCurrencies.ViewModels;

public sealed class SetCountryCurrencyActiveViewModel
{
    public int CountryCurrencyId { get; init; }

    public bool IsActive { get; init; }
}