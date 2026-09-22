CREATE PROCEDURE [dbo].[Currencies_GetById]
    @CurrencyId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF @CurrencyId <= 0
        THROW 50023, 'Currency ID must be greater than zero.', 1;

    SELECT
        [CurrencyId],
        [Name],
        [Code],
        [Symbol],
        [IsActive]
    FROM [dbo].[Currencies]
    WHERE [CurrencyId] = @CurrencyId;
END;