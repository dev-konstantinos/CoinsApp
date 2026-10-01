namespace CoinsApp.BLL.Features.Countries.ViewModels;

public sealed class UpdateCountryViewModel
{
    public int CountryId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Code { get; init; } = string.Empty;
    public bool IsActive { get; init; }
}