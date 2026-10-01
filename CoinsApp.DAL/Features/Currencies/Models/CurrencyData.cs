namespace CoinsApp.DAL.Features.Currencies.Models;

public sealed class CurrencyData
{
    public int CurrencyId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Code { get; init; } = string.Empty;
    public string? Symbol { get; init; }
    public bool IsActive { get; init; }
}