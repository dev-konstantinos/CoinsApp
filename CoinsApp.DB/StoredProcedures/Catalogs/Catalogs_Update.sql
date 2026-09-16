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

    IF @CatalogId <= 0
    BEGIN
        RAISERROR('CatalogId must be greater than zero.', 16, 1);
        RETURN;
    END;

    SET @Name = NULLIF(LTRIM(RTRIM(@Name)), N'');
    SET @ShortName = NULLIF(LTRIM(RTRIM(@ShortName)), N'');
    SET @Publisher = NULLIF(LTRIM(RTRIM(@Publisher)), N'');
    SET @Description = NULLIF(LTRIM(RTRIM(@Description)), N'');

    IF @Name IS NULL
    BEGIN
        RAISERROR('Catalog name is required.', 16, 1);
        RETURN;
    END;

    UPDATE [dbo].[Catalogs]
    SET
        [Name] = @Name,
        [ShortName] = @ShortName,
        [Publisher] = @Publisher,
        [Description] = @Description,
        [IsActive] = @IsActive
    WHERE [CatalogId] = @CatalogId;

    IF @@ROWCOUNT = 0
    BEGIN
        RAISERROR('Catalog was not found.', 16, 1);
        RETURN;
    END;

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