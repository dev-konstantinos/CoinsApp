CREATE PROCEDURE [dbo].[Currencies_SetActive]
    @CurrencyId INT,
    @IsActive BIT
AS
BEGIN
    SET NOCOUNT ON;

    IF @CurrencyId <= 0
        THROW 50029, 'Currency ID must be greater than zero.', 1;

    IF NOT EXISTS
    (
        SELECT 1
        FROM [dbo].[Currencies]
        WHERE [CurrencyId] = @CurrencyId
    )
        THROW 50030, 'Currency not found.', 1;

    UPDATE [dbo].[Currencies]
    SET
        [IsActive] = @IsActive
    WHERE [CurrencyId] = @CurrencyId;

    SELECT
        [CurrencyId],
        [Name],
        [Code],
        [Symbol],
        [IsActive]
    FROM [dbo].[Currencies]
    WHERE [CurrencyId] = @CurrencyId;
END;