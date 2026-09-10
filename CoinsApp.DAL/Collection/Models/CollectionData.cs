namespace CoinsApp.DAL.Collection.Models;

public sealed class CollectionData
{
    public int CollectionId { get; init; }

    public int UserId { get; init; }

    public string UserName { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public bool IsActive { get; init; }

    public DateTime CreatedAt { get; init; }

    public int CoinCount { get; init; }
}