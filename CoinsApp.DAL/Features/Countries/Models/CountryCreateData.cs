namespace CoinsApp.DAL.Features.Countries.Models;

public sealed class CountryCreateData
{
    public string Name { get; init; } = string.Empty;
    public string Code { get; init; } = string.Empty;
    public bool IsActive { get; init; } = true;
}