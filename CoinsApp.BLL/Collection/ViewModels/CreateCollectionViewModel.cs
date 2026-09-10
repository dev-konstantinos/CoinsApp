namespace CoinsApp.BLL.Collection.ViewModels;

public sealed class CreateCollectionViewModel
{
    public int UserId { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public bool IsActive { get; init; } = true;
}