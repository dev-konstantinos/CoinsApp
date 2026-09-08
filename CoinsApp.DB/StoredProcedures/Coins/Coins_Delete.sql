CREATE PROCEDURE [dbo].[Coins_Delete]
    @CoinId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF NOT EXISTS
        (
            SELECT 1
            FROM [dbo].[Coins]
            WHERE [CoinId] = @CoinId
        )
        BEGIN
            ROLLBACK TRANSACTION;
            RAISERROR ('Coin not found.', 16, 1);
            RETURN;
        END;

        DELETE FROM [dbo].[CatalogEntries]
        WHERE [CoinId] = @CoinId;

        DELETE FROM [dbo].[CoinImages]
        WHERE [CoinId] = @CoinId;

        DELETE FROM [dbo].[PriceHistory]
        WHERE [CoinId] = @CoinId;

        DELETE FROM [dbo].[Purchases]
        WHERE [CoinId] = @CoinId;

        DELETE FROM [dbo].[Sales]
        WHERE [CoinId] = @CoinId;

        DELETE FROM [dbo].[Coins]
        WHERE [CoinId] = @CoinId;

        COMMIT TRANSACTION;

        SELECT @CoinId AS [CoinId];
    END TRY
    BEGIN CATCH

        IF XACT_STATE() <> 0
        BEGIN
            ROLLBACK TRANSACTION;
        END;

        THROW;
    END CATCH;
END;
GO