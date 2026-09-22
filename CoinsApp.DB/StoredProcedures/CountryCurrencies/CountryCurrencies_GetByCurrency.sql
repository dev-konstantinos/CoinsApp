CREATE PROCEDURE [dbo].[CountryCurrencies_GetByCurrency]
    @CurrencyId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF @CurrencyId <= 0
        THROW 50107, 'Currency ID must be greater than zero.', 1;

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

    WHERE cc.[CurrencyId] = @CurrencyId

    ORDER BY
        c.[Name],
        cc.[CountryCurrencyId];
END;