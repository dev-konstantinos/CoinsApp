namespace CoinsApp.DAL.Mints.Models;

public sealed class MintCreateData
{
    public int CountryId { get; init; }

    public string Name { get; init; } = string.Empty;
    public string? Code { get; init; }
    public string? City { get; init; }
}