CREATE PROCEDURE [dbo].[CatalogEntries_Create]
    @CatalogId INT,
    @CoinId INT,
    @CatalogNumber NVARCHAR(50),
    @Notes NVARCHAR(1000) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF @CatalogId <= 0
    BEGIN
        RAISERROR('CatalogId must be greater than zero.', 16, 1);
        RETURN;
    END;

    IF @CoinId <= 0
    BEGIN
        RAISERROR('CoinId must be greater than zero.', 16, 1);
        RETURN;
    END;

    SET @CatalogNumber = NULLIF(LTRIM(RTRIM(@CatalogNumber)), N'');
    SET @Notes = NULLIF(LTRIM(RTRIM(@Notes)), N'');

    IF @CatalogNumber IS NULL
    BEGIN
        RAISERROR('Catalog number is required.', 16, 1);
        RETURN;
    END;

    INSERT INTO [dbo].[CatalogEntries]
    (
        [CatalogId],
        [CoinId],
        [CatalogNumber],
        [Notes]
    )
    VALUES
    (
        @CatalogId,
        @CoinId,
        @CatalogNumber,
        @Notes
    );

    SELECT
        [CatalogEntryId],
        [CatalogId],
        [CoinId],
        [CatalogNumber],
        [Notes]
    FROM [dbo].[CatalogEntries]
    WHERE [CatalogEntryId] = CONVERT(INT, SCOPE_IDENTITY());
END;
GO