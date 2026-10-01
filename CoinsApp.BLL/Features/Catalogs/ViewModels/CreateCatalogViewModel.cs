namespace CoinsApp.BLL.Features.Catalogs.ViewModels;

public sealed class CreateCatalogViewModel
{
    public string Name { get; init; } = string.Empty;

    public string? ShortName { get; init; }

    public string? Publisher { get; init; }

    public string? Description { get; init; }

    public bool IsActive { get; init; } = true;
}