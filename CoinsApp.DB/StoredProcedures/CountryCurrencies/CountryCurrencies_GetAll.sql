CREATE PROCEDURE [dbo].[CountryCurrencies_GetAll]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        [CountryCurrencyId],
        [CountryId],
        [CurrencyId],
        [IsActive]
    FROM [dbo].[CountryCurrencies]
    ORDER BY
        [CountryId],
        [CurrencyId],
        [CountryCurrencyId];
END;