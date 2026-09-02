CREATE PROCEDURE [dbo].[PriceHistory_Delete]
    @PriceHistoryId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        ------------------------------------------------------------
        -- 1. CoinId des History-Eintrags ermitteln
        ------------------------------------------------------------

        DECLARE @CoinId INT;

        SELECT
            @CoinId = [CoinId]
        FROM [dbo].[PriceHistory]
        WHERE [PriceHistoryId] = @PriceHistoryId;

        IF @CoinId IS NULL
        BEGIN
            IF XACT_STATE() <> 0
            BEGIN
                ROLLBACK TRANSACTION;
            END;

            RAISERROR ('Price history entry not found.', 16, 1);
            RETURN;
        END;


        ------------------------------------------------------------
        -- 2. History-Eintrag löschen
        ------------------------------------------------------------

        DELETE FROM [dbo].[PriceHistory]
        WHERE [PriceHistoryId] = @PriceHistoryId;


        ------------------------------------------------------------
        -- 3. Neuesten verbleibenden History-Eintrag ermitteln
        --
        -- Regel:
        --   1. höchstes PriceDate
        --   2. bei gleichem PriceDate:
        --      höchste PriceHistoryId
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
        -- 4. Coins.CurrentPrice synchronisieren
        --
        -- Wenn keine History mehr vorhanden ist,
        -- werden alle drei Werte NULL.
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
        -- 6. Gelöschte ID zurückgeben
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