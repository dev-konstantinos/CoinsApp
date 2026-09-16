CREATE PROCEDURE [dbo].[CatalogEntries_GetAll]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        [CatalogEntryId],
        [CatalogId],
        [CoinId],
        [CatalogNumber],
        [Notes]
    FROM [dbo].[CatalogEntries]
    ORDER BY
        [CatalogId] ASC,
        [CatalogNumber] ASC,
        [CatalogEntryId] ASC;
END;
GO