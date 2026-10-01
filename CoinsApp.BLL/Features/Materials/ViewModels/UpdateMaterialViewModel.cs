namespace CoinsApp.BLL.Features.Materials.ViewModels;

public sealed class UpdateMaterialViewModel
{
    public int MaterialId { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? Symbol { get; init; }

    public bool IsPreciousMetal { get; init; }

    public bool IsActive { get; init; }
}