CREATE PROCEDURE [dbo].[CoinImages_GetAll]
AS
BEGIN
    SET NOCOUNT ON;

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
    ORDER BY
        [CoinId] ASC,
        [SortOrder] ASC,
        [CoinImageId] ASC;
END;
GO