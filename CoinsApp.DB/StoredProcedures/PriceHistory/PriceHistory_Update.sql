CREATE PROCEDURE [dbo].[PriceHistory_Update]
    @PriceHistoryId INT,
    @Price DECIMAL(19,4),
    @CurrencyId INT,
    @PriceDate DATETIME2(0),
    @Source NVARCHAR(200) = NULL,
    @Notes NVARCHAR(1000) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        ------------------------------------------------------------
        -- 1. Determine and lock the Coin
        ------------------------------------------------------------

        DECLARE @CoinId INT;

        SELECT
            @CoinId = [CoinId]
        FROM [dbo].[PriceHistory] WITH (UPDLOCK, HOLDLOCK)
        WHERE [PriceHistoryId] = @PriceHistoryId;

        IF @CoinId IS NULL
        BEGIN
            ROLLBACK TRANSACTION;
            RAISERROR ('Price history entry not found.', 16, 1);
            RETURN;
        END;

        IF @PriceDate > SYSUTCDATETIME()
        BEGIN
            ROLLBACK TRANSACTION;
            RAISERROR ('PriceDate cannot be in the future.', 16, 1);
            RETURN;
        END;


        ------------------------------------------------------------
        -- 2. Update PriceHistory
        ------------------------------------------------------------

        UPDATE [dbo].[PriceHistory]
        SET
            [Price] = @Price,
            [CurrencyId] = @CurrencyId,
            [PriceDate] = @PriceDate,
            [Source] = @Source,
            [Notes] = @Notes
        WHERE [PriceHistoryId] = @PriceHistoryId;


        ------------------------------------------------------------
        -- 3. Find the newest price history entry
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
        -- 4. Synchronize Coins.CurrentPrice
        ------------------------------------------------------------

        UPDATE [dbo].[Coins]
        SET
            [CurrentPrice] = @CurrentPrice,
            [CurrentPriceCurrencyId] = @CurrentPriceCurrencyId,
            [CurrentPriceDate] = @CurrentPriceDate
        WHERE [CoinId] = @CoinId;


        ------------------------------------------------------------
        -- 5. Commit
        ------------------------------------------------------------

        COMMIT TRANSACTION;


        ------------------------------------------------------------
        -- 6. Return updated ID
        ------------------------------------------------------------

        SELECT
            [PriceHistoryId]
        FROM [dbo].[PriceHistory]
        WHERE [PriceHistoryId] = @PriceHistoryId;

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