namespace CoinsApp.DAL.Features.Countries.Models;

public sealed class CountrySetActiveData
{
    public int CountryId { get; init; }
    public bool IsActive { get; init; }
}