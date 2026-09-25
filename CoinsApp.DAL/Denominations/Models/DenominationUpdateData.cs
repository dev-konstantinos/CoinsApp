namespace CoinsApp.DAL.Denominations.Models;

public sealed class DenominationUpdateData
{
    public int DenominationId { get; init; }

    public int CurrencyId { get; init; }

    public decimal Value { get; init; }

    public string DisplayName { get; init; } = string.Empty;

    public bool IsActive { get; init; }
}