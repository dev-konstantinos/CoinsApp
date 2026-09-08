namespace CoinsApp.DAL.Coins.Models;

public sealed class CoinData
{
    public int CoinId { get; init; }
    public int? OwnerId { get; init; }
    public string? OwnerName { get; init; }
    public int CollectionId { get; init; }
    public int CountryId { get; init; }
    public string CountryName { get; init; } = string.Empty;

    public int CurrencyId { get; init; }
    public string CurrencyCode { get; init; } = string.Empty;
    public string CurrencyName { get; init; } = string.Empty;

    public int DenominationId { get; init; }
    public string DenominationDisplayName { get; init; } = string.Empty;

    public int? MintId { get; init; }
    public string? MintName { get; init; }

    public int? MaterialId { get; init; }
    public string? MaterialName { get; init; }

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

    public decimal? CurrentPrice { get; init; }
    public int? CurrentPriceCurrencyId { get; init; }
    public string? CurrentPriceCurrencyCode { get; init; }
    public DateTime? CurrentPriceDate { get; init; }

    public string? Notes { get; init; }

    public DateTime CreatedAt { get; init; }
}