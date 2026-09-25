CREATE PROCEDURE [dbo].[Denominations_Create]
    @CurrencyId INT,
    @Value DECIMAL(18, 4),
    @IsActive BIT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [dbo].[Denominations]
    (
        [CurrencyId],
        [Value],
        [IsActive]
    )
    VALUES
    (
        @CurrencyId,
        @Value,
        @IsActive
    );

    DECLARE @DenominationId INT =
        CAST(SCOPE_IDENTITY() AS INT);

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