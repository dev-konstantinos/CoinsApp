namespace CoinsApp.DAL.Features.Mints.Models;

public sealed class MintUpdateData
{
    public int MintId { get; init; }
    public int CountryId { get; init; }

    public string Name { get; init; } = string.Empty;
    public string? Code { get; init; }
    public string? City { get; init; }

    public bool IsActive { get; init; }
}