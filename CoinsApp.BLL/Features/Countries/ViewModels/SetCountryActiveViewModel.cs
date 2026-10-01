namespace CoinsApp.BLL.Features.Countries.ViewModels;

public sealed class SetCountryActiveViewModel
{
    public int CountryId { get; init; }
    public bool IsActive { get; init; }
}