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

    SET @Name = NULLIF(LTRIM(RTRIM(@Name)), N'');
    SET @ShortName = NULLIF(LTRIM(RTRIM(@ShortName)), N'');
    SET @Publisher = NULLIF(LTRIM(RTRIM(@Publisher)), N'');
    SET @Description = NULLIF(LTRIM(RTRIM(@Description)), N'');

    IF @Name IS NULL
    BEGIN
        RAISERROR('Catalog name is required.', 16, 1);
        RETURN;
    END;

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