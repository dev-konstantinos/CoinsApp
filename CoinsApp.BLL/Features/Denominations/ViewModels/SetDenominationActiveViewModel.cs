namespace CoinsApp.BLL.Features.Denominations.ViewModels;

public sealed class SetDenominationActiveViewModel
{
    public int DenominationId { get; init; }

    public bool IsActive { get; init; }
}