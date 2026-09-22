CREATE PROCEDURE [dbo].[CountryCurrencies_GetByCountry]
    @CountryId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF @CountryId <= 0
        THROW 50106, 'Country ID must be greater than zero.', 1;

    SELECT
        [CountryCurrencyId],
        [CountryId],
        [CurrencyId],
        [IsActive]
    FROM [dbo].[CountryCurrencies]
    WHERE [CountryId] = @CountryId
    ORDER BY
        [CurrencyId],
        [CountryCurrencyId];
END;