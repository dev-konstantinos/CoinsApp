CREATE PROCEDURE [dbo].[CatalogEntries_Delete]
    @CatalogEntryId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF @CatalogEntryId <= 0
        RETURN;

    DELETE FROM [dbo].[CatalogEntries]
    WHERE [CatalogEntryId] = @CatalogEntryId;

    SELECT
        CAST(CASE WHEN @@ROWCOUNT > 0 THEN 1 ELSE 0 END AS BIT) AS [Success];
END;
GO