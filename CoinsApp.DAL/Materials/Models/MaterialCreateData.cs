namespace CoinsApp.DAL.Materials.Models;

public sealed class MaterialCreateData
{
    public string Name { get; init; } = string.Empty;
    public string? Symbol { get; init; }
    public bool IsPreciousMetal { get; init; }
}
