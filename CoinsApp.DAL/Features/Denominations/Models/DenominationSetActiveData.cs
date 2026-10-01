namespace CoinsApp.DAL.Features.Denominations.Models;

public sealed class DenominationSetActiveData
{
    public int DenominationId { get; init; }

    public bool IsActive { get; init; }
}