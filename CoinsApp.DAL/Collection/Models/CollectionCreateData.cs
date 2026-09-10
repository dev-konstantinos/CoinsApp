namespace CoinsApp.DAL.Collection.Models;

public sealed class CollectionCreateData
{
    public int UserId { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public bool IsActive { get; init; } = true;
}