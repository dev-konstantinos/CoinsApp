CREATE PROCEDURE [dbo].[CatalogEntries_GetByCoin]
    @CoinId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF @CoinId <= 0
        RETURN;

    SELECT
        [CatalogEntryId],
        [CatalogId],
        [CoinId],
        [CatalogNumber],
        [Notes]
    FROM [dbo].[CatalogEntries]
    WHERE [CoinId] = @CoinId
    ORDER BY
        [CatalogId] ASC,
        [CatalogNumber] ASC,
        [CatalogEntryId] ASC;
END;
GO