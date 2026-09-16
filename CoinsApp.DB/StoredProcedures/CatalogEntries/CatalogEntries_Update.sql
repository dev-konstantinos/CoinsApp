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

    SET @CatalogNumber = NULLIF(LTRIM(RTRIM(@CatalogNumber)), N'');
    SET @Notes = NULLIF(LTRIM(RTRIM(@Notes)), N'');

    IF @CatalogEntryId <= 0
        RETURN;

    IF @CatalogId <= 0
        RETURN;

    IF @CoinId <= 0
        RETURN;

    IF @CatalogNumber IS NULL
        RETURN;

    UPDATE [dbo].[CatalogEntries]
    SET
        [CatalogId] = @CatalogId,
        [CoinId] = @CoinId,
        [CatalogNumber] = @CatalogNumber,
        [Notes] = @Notes
    WHERE [CatalogEntryId] = @CatalogEntryId;

    IF @@ROWCOUNT = 0
        RETURN;

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