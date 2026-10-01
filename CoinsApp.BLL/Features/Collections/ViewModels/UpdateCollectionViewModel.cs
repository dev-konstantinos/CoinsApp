namespace CoinsApp.BLL.Features.Collections.ViewModels;

public sealed class UpdateCollectionViewModel
{
    public int CollectionId { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public bool IsActive { get; init; }
}