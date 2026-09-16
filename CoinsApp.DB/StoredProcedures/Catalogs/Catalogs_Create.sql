CREATE PROCEDURE [dbo].[Catalogs_Create]
    @Name NVARCHAR(150),
    @ShortName NVARCHAR(50) = NULL,
    @Publisher NVARCHAR(200) = NULL,
    @Description NVARCHAR(1000) = NULL,
    @IsActive BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    INSERT INTO [dbo].[Catalogs]
    (
        [Name],
        [ShortName],
        [Publisher],
        [Description],
        [IsActive]
    )
    VALUES
    (
        @Name,
        @ShortName,
        @Publisher,
        @Description,
        @IsActive
    );

    SELECT
        [CatalogId],
        [Name],
        [ShortName],
        [Publisher],
        [Description],
        [IsActive]
    FROM [dbo].[Catalogs]
    WHERE [CatalogId] = CONVERT(INT, SCOPE_IDENTITY());
END;
GO