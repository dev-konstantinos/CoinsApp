CREATE PROCEDURE [dbo].[Denominations_GetAll]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        d.[DenominationId],
        d.[CurrencyId],
        c.[Name] AS [CurrencyName],
        c.[Code] AS [CurrencyCode],
        d.[Value],
        d.[IsActive]
    FROM [dbo].[Denominations] AS d
    INNER JOIN [dbo].[Currencies] AS c
        ON c.[CurrencyId] = d.[CurrencyId]
    ORDER BY
        c.[Name],
        d.[Value],
        d.[DenominationId];
END;