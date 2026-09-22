namespace CoinsApp.BLL.Currencies.ViewModels;

public sealed class CurrencyListItemViewModel
{
    public int CurrencyId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Code { get; init; } = string.Empty;
    public string? Symbol { get; init; }
    public bool IsActive { get; init; }
}