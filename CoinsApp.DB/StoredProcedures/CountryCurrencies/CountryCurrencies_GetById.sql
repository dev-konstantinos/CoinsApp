CREATE PROCEDURE [dbo].[CountryCurrencies_GetById]
    @CountryCurrencyId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF @CountryCurrencyId <= 0
        THROW 50105, 'Country-currency ID must be greater than zero.', 1;

    SELECT
        [CountryCurrencyId],
        [CountryId],
        [CurrencyId],
        [IsActive]
    FROM [dbo].[CountryCurrencies]
    WHERE [CountryCurrencyId] = @CountryCurrencyId;
END;