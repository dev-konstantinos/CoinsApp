namespace CoinsApp.BLL.Features.Mints.ViewModels;

public sealed class MintListItemViewModel
{
    public int MintId { get; init; }

    public int CountryId { get; init; }

    public string CountryName { get; init; } = string.Empty;
    public string CountryCode { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;
    public string? Code { get; init; }
    public string? City { get; init; }

    public bool IsActive { get; init; }
}