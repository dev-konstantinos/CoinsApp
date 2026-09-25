CREATE PROCEDURE [dbo].[Denominations_GetByCurrency]
    @CurrencyId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        d.[DenominationId],
        d.[CurrencyId],
        c.[Name] AS [CurrencyName],
        c.[Code] AS [CurrencyCode],
        d.[Value],
        d.[DisplayName],
        d.[IsActive]
    FROM [dbo].[Denominations] AS d
    INNER JOIN [dbo].[Currencies] AS c
        ON c.[CurrencyId] = d.[CurrencyId]
    WHERE d.[CurrencyId] = @CurrencyId
    ORDER BY
        d.[Value],
        d.[DenominationId];
END;