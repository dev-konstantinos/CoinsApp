CREATE PROCEDURE [dbo].[Coins_GetAll]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        c.CoinId,
        c.CollectionId,

        c.CountryId,
        country.Name AS CountryName,

        c.CurrencyId,
        currency.Code AS CurrencyCode,
        currency.Name AS CurrencyName,

        c.DenominationId,
        denomination.DisplayName AS DenominationDisplayName,

        c.MintId,
        mint.Name AS MintName,

        c.MaterialId,
        material.Name AS MaterialName,

        c.Year,
        c.MintMark,

        c.Fineness,
        c.Weight,
        c.Diameter,
        c.Thickness,

        c.Shape,
        c.Description,
        c.Designer,
        c.Mintage,

        c.Condition,
        c.Grade,
        c.GradingCompany,
        c.GradingCertificateNumber,

        c.CurrentPrice,
        c.CurrentPriceCurrencyId,
        priceCurrency.Code AS CurrentPriceCurrencyCode,
        c.CurrentPriceDate,

        c.OwnerId,
        owner.Name AS OwnerName,

        c.Notes,
        c.CreatedAt

    FROM dbo.Coins AS c

    INNER JOIN dbo.Countries AS country
        ON country.CountryId = c.CountryId

    INNER JOIN dbo.Currencies AS currency
        ON currency.CurrencyId = c.CurrencyId

    INNER JOIN dbo.Denominations AS denomination
        ON denomination.DenominationId = c.DenominationId
        AND denomination.CurrencyId = c.CurrencyId

    LEFT JOIN dbo.Mints AS mint
        ON mint.MintId = c.MintId
        AND mint.CountryId = c.CountryId

    LEFT JOIN dbo.Materials AS material
        ON material.MaterialId = c.MaterialId

    LEFT JOIN dbo.Currencies AS priceCurrency
        ON priceCurrency.CurrencyId = c.CurrentPriceCurrencyId
        
    LEFT JOIN dbo.Contacts AS owner
        ON owner.ContactId = c.OwnerId

    ORDER BY
        country.Name,
        c.Year,
        denomination.Value,
        c.CoinId;
END;
GO