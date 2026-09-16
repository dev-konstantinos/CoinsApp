CREATE PROCEDURE [dbo].[Catalogs_Delete]
    @CatalogId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF EXISTS
    (
        SELECT 1
        FROM [dbo].[CatalogEntries]
        WHERE [CatalogId] = @CatalogId
    )
    BEGIN
        SELECT
            CAST(0 AS BIT) AS [Success];

        RETURN;
    END;

    DELETE FROM [dbo].[Catalogs]
    WHERE [CatalogId] = @CatalogId;

    SELECT
        CAST(CASE WHEN @@ROWCOUNT > 0 THEN 1 ELSE 0 END AS BIT) AS [Success];
END;
GO