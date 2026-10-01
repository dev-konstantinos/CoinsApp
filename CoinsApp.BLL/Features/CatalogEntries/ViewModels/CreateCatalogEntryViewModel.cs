namespace CoinsApp.BLL.Features.CatalogEntries.ViewModels;

public sealed class CreateCatalogEntryViewModel
{
    public int CatalogId { get; init; }

    public int CoinId { get; init; }

    public string CatalogNumber { get; init; } = string.Empty;

    public string? Notes { get; init; }
}