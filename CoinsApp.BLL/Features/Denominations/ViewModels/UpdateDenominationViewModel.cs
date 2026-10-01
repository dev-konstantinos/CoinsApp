namespace CoinsApp.BLL.Features.Denominations.ViewModels;

public sealed class UpdateDenominationViewModel
{
    public int DenominationId { get; init; }

    public int CurrencyId { get; init; }

    public decimal Value { get; init; }

    public string DisplayName { get; init; } = string.Empty;

    public bool IsActive { get; init; }
}