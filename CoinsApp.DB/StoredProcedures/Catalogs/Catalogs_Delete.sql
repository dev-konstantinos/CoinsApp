CREATE PROCEDURE [dbo].[Catalogs_Delete]
    @CatalogId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF @CatalogId <= 0
    BEGIN
        RAISERROR('CatalogId must be greater than zero.', 16, 1);
        RETURN;
    END;

    IF NOT EXISTS
    (
        SELECT 1
        FROM [dbo].[Catalogs]
        WHERE [CatalogId] = @CatalogId
    )
    BEGIN
        RAISERROR('Catalog was not found.', 16, 1);
        RETURN;
    END;

    IF EXISTS
    (
        SELECT 1
        FROM [dbo].[CatalogEntries]
        WHERE [CatalogId] = @CatalogId
    )
    BEGIN
        RAISERROR(
            'Catalog cannot be deleted because catalog entries exist.',
            16,
            1
        );
        RETURN;
    END;

    DELETE FROM [dbo].[Catalogs]
    WHERE [CatalogId] = @CatalogId;

    SELECT
        @CatalogId AS [CatalogId];
END;
GO