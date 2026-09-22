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
        [CountryCurrencyId],
        [CountryId],
        [CurrencyId],
        [IsActive]
    FROM [dbo].[CountryCurrencies]
    WHERE [CountryCurrencyId] = @CountryCurrencyId;
END;