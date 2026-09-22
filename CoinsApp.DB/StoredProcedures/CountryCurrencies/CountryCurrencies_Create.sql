CREATE PROCEDURE [dbo].[CountryCurrencies_Create]
    @CountryId INT,
    @CurrencyId INT,
    @IsActive BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    IF @CountryId <= 0
        THROW 50100, 'Country ID must be greater than zero.', 1;

    IF @CurrencyId <= 0
        THROW 50101, 'Currency ID must be greater than zero.', 1;

    IF NOT EXISTS
    (
        SELECT 1
        FROM [dbo].[Countries]
        WHERE [CountryId] = @CountryId
    )
        THROW 50102, 'Country not found.', 1;

    IF NOT EXISTS
    (
        SELECT 1
        FROM [dbo].[Currencies]
        WHERE [CurrencyId] = @CurrencyId
    )
        THROW 50103, 'Currency not found.', 1;

    IF EXISTS
    (
        SELECT 1
        FROM [dbo].[CountryCurrencies]
        WHERE [CountryId] = @CountryId
          AND [CurrencyId] = @CurrencyId
    )
        THROW 50104, 'Country-currency relationship already exists.', 1;

    INSERT INTO [dbo].[CountryCurrencies]
    (
        [CountryId],
        [CurrencyId],
        [IsActive]
    )
    VALUES
    (
        @CountryId,
        @CurrencyId,
        @IsActive
    );

    SELECT
        [CountryCurrencyId],
        [CountryId],
        [CurrencyId],
        [IsActive]
    FROM [dbo].[CountryCurrencies]
    WHERE [CountryCurrencyId] = SCOPE_IDENTITY();
END;