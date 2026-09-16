CREATE PROCEDURE [dbo].[Catalogs_GetById]
    @CatalogId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        [CatalogId],
        [Name],
        [ShortName],
        [Publisher],
        [Description],
        [IsActive]
    FROM [dbo].[Catalogs]
    WHERE [CatalogId] = @CatalogId;
END;
GO