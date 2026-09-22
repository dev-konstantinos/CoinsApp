CREATE PROCEDURE [dbo].[CountryCurrencies_GetByCountry]
    @CountryId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF @CountryId <= 0
        THROW 50106, 'Country ID must be greater than zero.', 1;

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

    WHERE cc.[CountryId] = @CountryId

    ORDER BY
        cur.[Name],
        cc.[CountryCurrencyId];
END;