CREATE PROCEDURE [dbo].[CoinImages_GetById]
    @CoinImageId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF @CoinImageId <= 0
    BEGIN
        RAISERROR('CoinImageId must be greater than zero.', 16, 1);
        RETURN;
    END;

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