CREATE PROCEDURE [dbo].[CatalogEntries_Delete]
    @CatalogEntryId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF @CatalogEntryId <= 0
    BEGIN
        RAISERROR('CatalogEntryId must be greater than zero.', 16, 1);
        RETURN;
    END;

    DELETE FROM [dbo].[CatalogEntries]
    WHERE [CatalogEntryId] = @CatalogEntryId;

    IF @@ROWCOUNT = 0
    BEGIN
        RAISERROR('Catalog entry was not found.', 16, 1);
        RETURN;
    END;

    SELECT
        @CatalogEntryId AS [CatalogEntryId];
END;
GO