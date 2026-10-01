namespace CoinsApp.DAL.Features.CountryCurrencies.Models;

public sealed class CountryCurrencyCreateData
{
    public int CountryId { get; init; }
    public int CurrencyId { get; init; }
    public bool IsActive { get; init; } = true;
}