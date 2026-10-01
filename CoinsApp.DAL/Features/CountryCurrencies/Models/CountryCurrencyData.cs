namespace CoinsApp.DAL.Features.CountryCurrencies.Models;

public sealed class CountryCurrencyData
{
    public int CountryCurrencyId { get; init; }

    public int CountryId { get; init; }
    public string CountryName { get; init; } = string.Empty;
    public string CountryCode { get; init; } = string.Empty;

    public int CurrencyId { get; init; }
    public string CurrencyName { get; init; } = string.Empty;
    public string CurrencyCode { get; init; } = string.Empty;
    public string? CurrencySymbol { get; init; }

    public bool IsActive { get; init; }
}