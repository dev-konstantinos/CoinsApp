namespace CoinsApp.BLL.Denominations.ViewModels;

public sealed class SetDenominationActiveViewModel
{
    public int DenominationId { get; init; }

    public bool IsActive { get; init; }
}