CREATE PROCEDURE [dbo].[PriceHistory_Create]
    @CoinId INT,
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

        IF NOT EXISTS
        (
            SELECT 1
            FROM [dbo].[Coins]
            WHERE [CoinId] = @CoinId
        )
        BEGIN
            IF XACT_STATE() <> 0
            BEGIN
                ROLLBACK TRANSACTION;
            END;

            RAISERROR ('Coin not found.', 16, 1);
            RETURN;
        END;

        INSERT INTO [dbo].[PriceHistory]
        (
            [CoinId],
            [Price],
            [CurrencyId],
            [PriceDate],
            [Source],
            [Notes]
        )
        VALUES
        (
            @CoinId,
            @Price,
            @CurrencyId,
            @PriceDate,
            @Source,
            @Notes
        );

        DECLARE @PriceHistoryId INT;

        SET @PriceHistoryId =
            CONVERT(INT, SCOPE_IDENTITY());

        /*
            Determine the newest price entry for the coin.

            Primary rule:
                newest PriceDate

            Tie-breaker:
                highest PriceHistoryId
        */

        DECLARE @LatestPriceHistoryId INT;

        SELECT TOP (1)
            @LatestPriceHistoryId = [PriceHistoryId]
        FROM [dbo].[PriceHistory]
        WHERE [CoinId] = @CoinId
        ORDER BY
            [PriceDate] DESC,
            [PriceHistoryId] DESC;

        /*
            Synchronize Coins.CurrentPrice only when
            the newly created entry is the newest entry.
        */

        IF @LatestPriceHistoryId = @PriceHistoryId
        BEGIN
            UPDATE [dbo].[Coins]
            SET
                [CurrentPrice] = @Price,
                [CurrentPriceCurrencyId] = @CurrencyId,
                [CurrentPriceDate] = @PriceDate
            WHERE [CoinId] = @CoinId;
        END;

        COMMIT TRANSACTION;

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