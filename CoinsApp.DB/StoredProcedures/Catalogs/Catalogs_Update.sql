CREATE PROCEDURE [dbo].[Catalogs_Update]
    @CatalogId INT,
    @Name NVARCHAR(150),
    @ShortName NVARCHAR(50) = NULL,
    @Publisher NVARCHAR(200) = NULL,
    @Description NVARCHAR(1000) = NULL,
    @IsActive BIT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    SET @Name = NULLIF(LTRIM(RTRIM(@Name)), N'');
    SET @ShortName = NULLIF(LTRIM(RTRIM(@ShortName)), N'');
    SET @Publisher = NULLIF(LTRIM(RTRIM(@Publisher)), N'');
    SET @Description = NULLIF(LTRIM(RTRIM(@Description)), N'');

    IF @CatalogId <= 0 OR @Name IS NULL
        RETURN;

    UPDATE [dbo].[Catalogs]
    SET
        [Name] = @Name,
        [ShortName] = @ShortName,
        [Publisher] = @Publisher,
        [Description] = @Description,
        [IsActive] = @IsActive
    WHERE [CatalogId] = @CatalogId;

    IF @@ROWCOUNT = 0
        RETURN;

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