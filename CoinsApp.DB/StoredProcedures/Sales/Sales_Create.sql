CREATE PROCEDURE [dbo].[Sales_Create]
    @CoinId INT,
    @SaleDate DATETIME2(0),
    @SalePrice DECIMAL(19,4),
    @CurrencyId INT,
    @BuyerId INT,
    @Notes NVARCHAR(2000) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @PreviousOwnerId INT;
    DECLARE @InitialOwnerId INT;
    DECLARE @CurrentOwnerId INT;
    DECLARE @SaleId INT;
    DECLARE @CurrentSaleId INT;
    DECLARE @CurrentBuyerId INT;

    BEGIN TRY
        BEGIN TRANSACTION;

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

        SELECT
            @PreviousOwnerId = [OwnerId],
            @InitialOwnerId = [InitialOwnerId]
        FROM [dbo].[Coins] WITH (UPDLOCK, HOLDLOCK)
        WHERE [CoinId] = @CoinId;

        IF @SaleDate > SYSUTCDATETIME()
        BEGIN
            ROLLBACK TRANSACTION;
            RAISERROR ('SaleDate cannot be in the future.', 16, 1);
            RETURN;
        END;

        IF @BuyerId <= 0
        BEGIN
            ROLLBACK TRANSACTION;
            RAISERROR ('BuyerId must be greater than zero.', 16, 1);
            RETURN;
        END;

        ------------------------------------------------------------
        -- Preserve the original owner only once.
        --
        -- If the original owner is already known, it must never
        -- be overwritten by a later Sale.
        --
        -- If both InitialOwnerId and OwnerId are NULL, the original
        -- owner remains unknown.
        ------------------------------------------------------------

        IF @InitialOwnerId IS NULL
           AND @PreviousOwnerId IS NOT NULL
        BEGIN
            UPDATE [dbo].[Coins]
            SET [InitialOwnerId] = [OwnerId]
            WHERE [CoinId] = @CoinId
              AND [InitialOwnerId] IS NULL;
        END;

        INSERT INTO [dbo].[Sales]
        (
            [CoinId],
            [SaleDate],
            [SalePrice],
            [CurrencyId],
            [PreviousOwnerId],
            [BuyerId],
            [Notes]
        )
        VALUES
        (
            @CoinId,
            @SaleDate,
            @SalePrice,
            @CurrencyId,
            @PreviousOwnerId,
            @BuyerId,
            @Notes
        );

        SET @SaleId =
            CONVERT(INT, SCOPE_IDENTITY());

        ------------------------------------------------------------
        -- Sales are ordered chronologically. Therefore the result
        -- is independent of the order in which Sales were entered.
        --
        -- The same ownership chain must be produced even when
        -- these Sales were entered in a different order.
        ------------------------------------------------------------

        SELECT
            @InitialOwnerId = [InitialOwnerId]
        FROM [dbo].[Coins] WITH (UPDLOCK, HOLDLOCK)
        WHERE [CoinId] = @CoinId;

        SET @CurrentOwnerId = @InitialOwnerId;

        DECLARE SalesCursor CURSOR LOCAL FAST_FORWARD FOR
            SELECT
                [SaleId],
                [BuyerId]
            FROM [dbo].[Sales]
            WHERE [CoinId] = @CoinId
            ORDER BY
                [SaleDate] ASC,
                [SaleId] ASC;

        OPEN SalesCursor;

        FETCH NEXT FROM SalesCursor
        INTO @CurrentSaleId, @CurrentBuyerId;

        WHILE @@FETCH_STATUS = 0
        BEGIN
            UPDATE [dbo].[Sales]
            SET [PreviousOwnerId] = @CurrentOwnerId
            WHERE [SaleId] = @CurrentSaleId;

            SET @CurrentOwnerId = @CurrentBuyerId;

            FETCH NEXT FROM SalesCursor
            INTO @CurrentSaleId, @CurrentBuyerId;
        END;

        CLOSE SalesCursor;
        DEALLOCATE SalesCursor;

        ------------------------------------------------------------
        -- Store the last buyer as the current owner.
        ------------------------------------------------------------

        UPDATE [dbo].[Coins]
        SET [OwnerId] = @CurrentOwnerId
        WHERE [CoinId] = @CoinId;

        COMMIT TRANSACTION;

        SELECT
            @SaleId AS [SaleId];

    END TRY
    BEGIN CATCH

        IF CURSOR_STATUS('local', 'SalesCursor') >= 0
        BEGIN
            CLOSE SalesCursor;
        END;

        IF CURSOR_STATUS('local', 'SalesCursor') >= -1
        BEGIN
            DEALLOCATE SalesCursor;
        END;

        IF XACT_STATE() <> 0
        BEGIN
            ROLLBACK TRANSACTION;
        END;

        THROW;
    END CATCH;
END;
GO