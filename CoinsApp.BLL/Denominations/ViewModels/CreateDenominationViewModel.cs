namespace CoinsApp.BLL.Denominations.ViewModels;

public sealed class CreateDenominationViewModel
{
    public int CurrencyId { get; init; }

    public decimal Value { get; init; }

    public string DisplayName { get; init; } = string.Empty;

    public bool IsActive { get; init; }
}