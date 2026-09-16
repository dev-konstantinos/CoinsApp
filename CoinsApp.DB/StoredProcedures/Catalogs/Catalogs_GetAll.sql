CREATE PROCEDURE [dbo].[Catalogs_GetAll]
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
    ORDER BY
        [Name] ASC,
        [CatalogId] ASC;
END;
GO