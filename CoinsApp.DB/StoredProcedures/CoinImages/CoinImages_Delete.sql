CREATE PROCEDURE [dbo].[CoinImages_Delete]
    @CoinImageId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF @CoinImageId <= 0
    BEGIN
        RAISERROR('CoinImageId must be greater than zero.', 16, 1);
        RETURN;
    END;

    IF NOT EXISTS
    (
        SELECT 1
        FROM [dbo].[CoinImages]
        WHERE [CoinImageId] = @CoinImageId
    )
    BEGIN
        RAISERROR('Coin image not found.', 16, 1);
        RETURN;
    END;

    DELETE FROM [dbo].[CoinImages]
    WHERE [CoinImageId] = @CoinImageId;

    SELECT
        @CoinImageId AS [CoinImageId];
END;
GO