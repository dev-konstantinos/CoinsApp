namespace CoinsApp.BLL.Features.Catalogs.ViewModels;

public sealed class CatalogListItemViewModel
{
    public int CatalogId { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? ShortName { get; init; }

    public string? Publisher { get; init; }

    public bool IsActive { get; init; }
}