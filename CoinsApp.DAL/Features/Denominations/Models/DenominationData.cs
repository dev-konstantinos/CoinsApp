namespace CoinsApp.DAL.Features.Denominations.Models;

public sealed class DenominationData
{
    public int DenominationId { get; init; }

    public int CurrencyId { get; init; }

    public string CurrencyName { get; init; } = string.Empty;

    public string CurrencyCode { get; init; } = string.Empty;

    public decimal Value { get; init; }

    public string DisplayName { get; init; } = string.Empty;

    public bool IsActive { get; init; }
}