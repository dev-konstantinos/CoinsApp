CREATE PROCEDURE [dbo].[CountryCurrencies_SetActive]
    @CountryCurrencyId INT,
    @IsActive BIT
AS
BEGIN
    SET NOCOUNT ON;

    IF @CountryCurrencyId <= 0
        THROW 50108, 'Country-currency ID must be greater than zero.', 1;

    IF NOT EXISTS
    (
        SELECT 1
        FROM [dbo].[CountryCurrencies]
        WHERE [CountryCurrencyId] = @CountryCurrencyId
    )
        THROW 50109, 'Country-currency relationship not found.', 1;

    UPDATE [dbo].[CountryCurrencies]
    SET
        [IsActive] = @IsActive
    WHERE [CountryCurrencyId] = @CountryCurrencyId;

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

    WHERE cc.[CountryCurrencyId] = @CountryCurrencyId;
END;