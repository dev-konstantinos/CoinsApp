CREATE PROCEDURE [dbo].[CatalogEntries_Create]
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

    IF @CatalogId <= 0
        RETURN;

    IF @CoinId <= 0
        RETURN;

    IF @CatalogNumber IS NULL
        RETURN;

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