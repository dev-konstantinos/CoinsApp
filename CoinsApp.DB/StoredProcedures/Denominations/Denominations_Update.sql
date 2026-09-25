CREATE PROCEDURE [dbo].[Denominations_Update]
    @DenominationId INT,
    @CurrencyId INT,
    @Value DECIMAL(18, 4),
    @IsActive BIT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE [dbo].[Denominations]
    SET
        [CurrencyId] = @CurrencyId,
        [Value] = @Value,
        [IsActive] = @IsActive
    WHERE [DenominationId] = @DenominationId;

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
    WHERE d.[DenominationId] = @DenominationId;
END;