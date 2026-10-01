namespace CoinsApp.BLL.Features.Currencies.ViewModels;

public sealed class UpdateCurrencyViewModel
{
    public int CurrencyId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Code { get; init; } = string.Empty;
    public string? Symbol { get; init; }
    public bool IsActive { get; init; }
}