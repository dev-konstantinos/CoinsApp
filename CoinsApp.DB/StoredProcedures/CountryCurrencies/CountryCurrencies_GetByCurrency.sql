CREATE PROCEDURE [dbo].[CountryCurrencies_GetByCurrency]
    @CurrencyId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF @CurrencyId <= 0
        THROW 50107, 'Currency ID must be greater than zero.', 1;

    SELECT
        [CountryCurrencyId],
        [CountryId],
        [CurrencyId],
        [IsActive]
    FROM [dbo].[CountryCurrencies]
    WHERE [CurrencyId] = @CurrencyId
    ORDER BY
        [CountryId],
        [CountryCurrencyId];
END;