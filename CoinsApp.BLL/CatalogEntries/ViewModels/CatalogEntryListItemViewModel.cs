namespace CoinsApp.BLL.CatalogEntries.ViewModels;

public sealed class CatalogEntryListItemViewModel
{
    public int CatalogEntryId { get; init; }

    public int CatalogId { get; init; }

    public int CoinId { get; init; }

    public string CatalogNumber { get; init; } = string.Empty;

    public string? Notes { get; init; }
}