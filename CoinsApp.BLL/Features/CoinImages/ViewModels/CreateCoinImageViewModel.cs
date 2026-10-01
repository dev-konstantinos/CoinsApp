namespace CoinsApp.BLL.Features.CoinImages.ViewModels;

public sealed class CreateCoinImageViewModel
{
    public int CoinId { get; init; }

    public string ImageType { get; init; } = string.Empty;

    public string FileName { get; init; } = string.Empty;

    public string FilePath { get; init; } = string.Empty;

    public string? Description { get; init; }

    public int SortOrder { get; init; }
}