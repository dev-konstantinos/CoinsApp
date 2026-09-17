CREATE PROCEDURE [dbo].[CoinImages_Create]
    @CoinId INT,
    @ImageType NVARCHAR(30),
    @FileName NVARCHAR(255),
    @FilePath NVARCHAR(1000),
    @Description NVARCHAR(500) = NULL,
    @SortOrder INT = 0
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF @CoinId <= 0
    BEGIN
        RAISERROR('CoinId must be greater than zero.', 16, 1);
        RETURN;
    END;

    IF NOT EXISTS
    (
        SELECT 1
        FROM [dbo].[Coins]
        WHERE [CoinId] = @CoinId
    )
    BEGIN
        RAISERROR('Coin not found.', 16, 1);
        RETURN;
    END;

    SET @ImageType = NULLIF(LTRIM(RTRIM(@ImageType)), N'');
    SET @FileName = NULLIF(LTRIM(RTRIM(@FileName)), N'');
    SET @FilePath = NULLIF(LTRIM(RTRIM(@FilePath)), N'');
    SET @Description = NULLIF(LTRIM(RTRIM(@Description)), N'');

    IF @ImageType IS NULL
    BEGIN
        RAISERROR('Image type is required.', 16, 1);
        RETURN;
    END;

    IF @FileName IS NULL
    BEGIN
        RAISERROR('File name is required.', 16, 1);
        RETURN;
    END;

    IF @FilePath IS NULL
    BEGIN
        RAISERROR('File path is required.', 16, 1);
        RETURN;
    END;

    IF @SortOrder < 0
    BEGIN
        RAISERROR('Sort order cannot be negative.', 16, 1);
        RETURN;
    END;

    INSERT INTO [dbo].[CoinImages]
    (
        [CoinId],
        [ImageType],
        [FileName],
        [FilePath],
        [Description],
        [SortOrder]
    )
    VALUES
    (
        @CoinId,
        @ImageType,
        @FileName,
        @FilePath,
        @Description,
        @SortOrder
    );

    DECLARE @CoinImageId INT;

    SET @CoinImageId =
        CONVERT(INT, SCOPE_IDENTITY());

    SELECT
        [CoinImageId],
        [CoinId],
        [ImageType],
        [FileName],
        [FilePath],
        [Description],
        [SortOrder],
        [CreatedAt]
    FROM [dbo].[CoinImages]
    WHERE [CoinImageId] = @CoinImageId;
END;
GO