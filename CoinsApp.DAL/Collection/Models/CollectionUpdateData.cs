namespace CoinsApp.DAL.Collection.Models;

public sealed class CollectionUpdateData
{
    public int CollectionId { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public bool IsActive { get; init; }
}