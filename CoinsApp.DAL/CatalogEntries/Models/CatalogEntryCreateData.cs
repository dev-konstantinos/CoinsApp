namespace CoinsApp.DAL.CatalogEntries.Models;

public sealed class CatalogEntryCreateData
{
    public int CatalogId { get; init; }

    public int CoinId { get; init; }

    public string CatalogNumber { get; init; } = string.Empty;

    public string? Notes { get; init; }
}