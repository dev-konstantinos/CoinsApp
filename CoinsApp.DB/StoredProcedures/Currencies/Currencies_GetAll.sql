CREATE PROCEDURE [dbo].[Currencies_GetAll]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        [CurrencyId],
        [Name],
        [Code],
        [Symbol],
        [IsActive]
    FROM [dbo].[Currencies]
    ORDER BY
        [Name],
        [CurrencyId];
END;