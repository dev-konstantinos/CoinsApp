CREATE PROCEDURE [dbo].[CatalogEntries_GetByCatalog]
    @CatalogId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF @CatalogId <= 0
        RETURN;

    SELECT
        [CatalogEntryId],
        [CatalogId],
        [CoinId],
        [CatalogNumber],
        [Notes]
    FROM [dbo].[CatalogEntries]
    WHERE [CatalogId] = @CatalogId
    ORDER BY
        [CatalogNumber] ASC,
        [CatalogEntryId] ASC;
END;
GO