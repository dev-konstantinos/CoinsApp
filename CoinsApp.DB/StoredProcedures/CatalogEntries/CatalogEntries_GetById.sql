CREATE PROCEDURE [dbo].[CatalogEntries_GetById]
    @CatalogEntryId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF @CatalogEntryId <= 0
        RETURN;

    SELECT
        [CatalogEntryId],
        [CatalogId],
        [CoinId],
        [CatalogNumber],
        [Notes]
    FROM [dbo].[CatalogEntries]
    WHERE [CatalogEntryId] = @CatalogEntryId;
END;
GO