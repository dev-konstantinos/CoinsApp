CREATE PROCEDURE [dbo].[CatalogEntries_Update]
    @CatalogEntryId INT,
    @CatalogId INT,
    @CoinId INT,
    @CatalogNumber NVARCHAR(50),
    @Notes NVARCHAR(1000) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF @CatalogEntryId <= 0
    BEGIN
        RAISERROR('CatalogEntryId must be greater than zero.', 16, 1);
        RETURN;
    END;

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

    UPDATE [dbo].[CatalogEntries]
    SET
        [CatalogId] = @CatalogId,
        [CoinId] = @CoinId,
        [CatalogNumber] = @CatalogNumber,
        [Notes] = @Notes
    WHERE [CatalogEntryId] = @CatalogEntryId;

    IF @@ROWCOUNT = 0
    BEGIN
        RAISERROR('Catalog entry was not found.', 16, 1);
        RETURN;
    END;

    SELECT
        [CatalogEntryId],
        [CatalogId],
        [CoinId],
        [CatalogNumber],
        [Notes]
    FROM [dbo].[CatalogEntries]
    WHERE [CatalogEntryId] = @CatalogEntryId;
END;
GO