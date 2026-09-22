namespace CoinsApp.DAL.Currencies.Models;

public sealed class CurrencyCreateData
{
    public string Name { get; init; } = string.Empty;
    public string Code { get; init; } = string.Empty;
    public string? Symbol { get; init; }
    public bool IsActive { get; init; } = true;
}