namespace CoinsApp.DAL.Currencies.Models;

public sealed class CurrencySetActiveData
{
    public int CurrencyId { get; init; }
    public bool IsActive { get; init; }
}