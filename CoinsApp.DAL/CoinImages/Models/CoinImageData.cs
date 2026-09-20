namespace CoinsApp.DAL.CoinImages.Models;

public sealed class CoinImageData
{
    public int CoinImageId { get; init; }

    public int CoinId { get; init; }

    public string ImageType { get; init; } = string.Empty;

    public string FileName { get; init; } = string.Empty;

    public string FilePath { get; init; } = string.Empty;

    public string? Description { get; init; }

    public int SortOrder { get; init; }

    public DateTime CreatedAt { get; init; }
}