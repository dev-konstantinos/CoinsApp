namespace CoinsApp.BLL.Catalogs.Models;

public sealed class CatalogCreateData
{
    public string Name { get; init; } = string.Empty;

    public string? ShortName { get; init; }

    public string? Publisher { get; init; }

    public string? Description { get; init; }

    public bool IsActive { get; init; } = true;
}