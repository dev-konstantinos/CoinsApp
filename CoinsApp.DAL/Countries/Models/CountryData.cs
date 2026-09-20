namespace CoinsApp.DAL.Countries.Models;

public sealed class CountryData
{
    public int CountryId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Code { get; init; } = string.Empty;
    public bool IsActive { get; init; }
}