CREATE PROCEDURE [dbo].[CountryCurrencies_GetAll]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        cc.[CountryCurrencyId],

        cc.[CountryId],
        c.[Name] AS [CountryName],
        c.[Code] AS [CountryCode],

        cc.[CurrencyId],
        cur.[Name] AS [CurrencyName],
        cur.[Code] AS [CurrencyCode],
        cur.[Symbol] AS [CurrencySymbol],

        cc.[IsActive]
    FROM [dbo].[CountryCurrencies] AS cc

    INNER JOIN [dbo].[Countries] AS c
        ON c.[CountryId] = cc.[CountryId]

    INNER JOIN [dbo].[Currencies] AS cur
        ON cur.[CurrencyId] = cc.[CurrencyId]

    ORDER BY
        c.[Name],
        cur.[Name],
        cc.[CountryCurrencyId];
END;