namespace CoinsApp.BLL.Features.CountryCurrencies.ViewModels;

public sealed class CreateCountryCurrencyViewModel
{
    public int CountryId { get; init; }

    public int CurrencyId { get; init; }

    public bool IsActive { get; init; } = true;
}