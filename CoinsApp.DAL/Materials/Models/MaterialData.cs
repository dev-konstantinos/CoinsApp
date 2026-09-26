namespace CoinsApp.DAL.Materials.Models;

public sealed class MaterialData
{
    public int MaterialId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Symbol { get; init; }
    public bool IsPreciousMetal { get; init; }
}
