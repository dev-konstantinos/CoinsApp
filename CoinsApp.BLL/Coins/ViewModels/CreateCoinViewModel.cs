namespace CoinsApp.BLL.Coins.ViewModels;

public sealed class CreateCoinViewModel
{
    public int CollectionId { get; init; }

    public int CountryId { get; init; }

    public int CurrencyId { get; init; }

    public int DenominationId { get; init; }

    public int? MintId { get; init; }

    public int? MaterialId { get; init; }

    public short? Year { get; init; }

    public string? MintMark { get; init; }

    public decimal? Fineness { get; init; }

    public decimal? Weight { get; init; }

    public decimal? Diameter { get; init; }

    public decimal? Thickness { get; init; }

    public string? Shape { get; init; }

    public string? Description { get; init; }

    public string? Designer { get; init; }

    public long? Mintage { get; init; }

    public string? Condition { get; init; }

    public string? Grade { get; init; }

    public string? GradingCompany { get; init; }

    public string? GradingCertificateNumber { get; init; }

    public string? Notes { get; init; }
}