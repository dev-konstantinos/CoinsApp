namespace CoinsApp.DAL.Features.CatalogEntries.Models;

public sealed class CatalogEntryUpdateData
{
    public int CatalogEntryId { get; init; }

    public int CatalogId { get; init; }

    public int CoinId { get; init; }

    public string CatalogNumber { get; init; } = string.Empty;

    public string? Notes { get; init; }
}