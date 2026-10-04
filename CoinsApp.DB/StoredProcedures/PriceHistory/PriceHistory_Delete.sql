CREATE PROCEDURE [dbo].[PriceHistory_Delete]
    @PriceHistoryId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        ------------------------------------------------------------
        -- 1. Determine the CoinId
        ------------------------------------------------------------

        DECLARE @CoinId INT;

        SELECT
            @CoinId = [CoinId]
        FROM [dbo].[PriceHistory]
        WHERE [PriceHistoryId] = @PriceHistoryId;

        IF @CoinId IS NULL
        BEGIN
            ROLLBACK TRANSACTION;
            RAISERROR ('Price history entry not found.', 16, 1);
            RETURN;
        END;


        ------------------------------------------------------------
        -- 2. Lock the Coin
        --
        -- All PriceHistory modifications for the same Coin
        -- use the Coin row as the synchronization point.
        ------------------------------------------------------------

        IF NOT EXISTS
        (
            SELECT 1
            FROM [dbo].[Coins] WITH (UPDLOCK, HOLDLOCK)
            WHERE [CoinId] = @CoinId
        )
        BEGIN
            ROLLBACK TRANSACTION;
            RAISERROR ('Coin not found.', 16, 1);
            RETURN;
        END;


        ------------------------------------------------------------
        -- 3. Lock the PriceHistory entry
        ------------------------------------------------------------

        IF NOT EXISTS
        (
            SELECT 1
            FROM [dbo].[PriceHistory] WITH (UPDLOCK, HOLDLOCK)
            WHERE [PriceHistoryId] = @PriceHistoryId
        )
        BEGIN
            ROLLBACK TRANSACTION;
            RAISERROR ('Price history entry not found.', 16, 1);
            RETURN;
        END;


        ------------------------------------------------------------
        -- 4. Delete PriceHistory entry
        ------------------------------------------------------------

        DELETE FROM [dbo].[PriceHistory]
        WHERE [PriceHistoryId] = @PriceHistoryId;


        ------------------------------------------------------------
        -- 5. Determine the newest remaining PriceHistory entry
        --
        -- Rule:
        --   1. highest PriceDate
        --   2. if PriceDate is equal:
        --      highest PriceHistoryId
        ------------------------------------------------------------

        DECLARE
            @CurrentPrice DECIMAL(19,4),
            @CurrentPriceCurrencyId INT,
            @CurrentPriceDate DATETIME2(0);

        SELECT TOP (1)
            @CurrentPrice = [Price],
            @CurrentPriceCurrencyId = [CurrencyId],
            @CurrentPriceDate = [PriceDate]
        FROM [dbo].[PriceHistory]
        WHERE [CoinId] = @CoinId
        ORDER BY
            [PriceDate] DESC,
            [PriceHistoryId] DESC;


        ------------------------------------------------------------
        -- 6. Synchronize Coins.CurrentPrice
        --
        -- If no PriceHistory remains, all three values become NULL.
        ------------------------------------------------------------

        UPDATE [dbo].[Coins]
        SET
            [CurrentPrice] = @CurrentPrice,
            [CurrentPriceCurrencyId] = @CurrentPriceCurrencyId,
            [CurrentPriceDate] = @CurrentPriceDate
        WHERE [CoinId] = @CoinId;


        ------------------------------------------------------------
        -- 7. Commit
        ------------------------------------------------------------

        COMMIT TRANSACTION;


        ------------------------------------------------------------
        -- 8. Return deleted ID
        ------------------------------------------------------------

        SELECT
            @PriceHistoryId AS [PriceHistoryId];

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