CREATE PROCEDURE [dbo].[CoinImages_GetByCoin]
    @CoinId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF @CoinId <= 0
    BEGIN
        RAISERROR('CoinId must be greater than zero.', 16, 1);
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
    WHERE [CoinId] = @CoinId
    ORDER BY
        [SortOrder] ASC,
        [CoinImageId] ASC;
END;
GO