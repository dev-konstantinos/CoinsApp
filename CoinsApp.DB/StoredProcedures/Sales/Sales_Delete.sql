CREATE PROCEDURE [dbo].[Sales_Delete]
    @SaleId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @CoinId INT;
    DECLARE @InitialOwnerId INT;
    DECLARE @CurrentOwnerId INT;
    DECLARE @CurrentSaleId INT;
    DECLARE @CurrentBuyerId INT;

    BEGIN TRY
        BEGIN TRANSACTION;

        ------------------------------------------------------------
        -- Locate the Sale and lock it for the duration of the
        -- transaction.
        ------------------------------------------------------------

        SELECT
            @CoinId = [CoinId]
        FROM [dbo].[Sales] WITH (UPDLOCK, HOLDLOCK)
        WHERE [SaleId] = @SaleId;

        IF @CoinId IS NULL
        BEGIN
            ROLLBACK TRANSACTION;
            RAISERROR('Sale not found.', 16, 1);
            RETURN;
        END;

        ------------------------------------------------------------
        -- Lock the related Coin.
        --
        -- The Coin is the synchronization point for all ownership-
        -- changing Sale operations.
        ------------------------------------------------------------

        IF NOT EXISTS
        (
            SELECT 1
            FROM [dbo].[Coins] WITH (UPDLOCK, HOLDLOCK)
            WHERE [CoinId] = @CoinId
        )
        BEGIN
            ROLLBACK TRANSACTION;
            RAISERROR('Coin not found.', 16, 1);
            RETURN;
        END;

        ------------------------------------------------------------
        -- InitialOwnerId is historical data.
        --
        -- DELETE must never change it.
        -- It is only read as the starting point for rebuilding the
        -- remaining ownership chain.
        ------------------------------------------------------------

        SELECT
            @InitialOwnerId = [InitialOwnerId]
        FROM [dbo].[Coins] WITH (UPDLOCK, HOLDLOCK)
        WHERE [CoinId] = @CoinId;

        ------------------------------------------------------------
        -- Delete the requested Sale.
        ------------------------------------------------------------

        DELETE FROM [dbo].[Sales]
        WHERE [SaleId] = @SaleId;

        ------------------------------------------------------------
        -- Rebuild the remaining ownership chain.
        --
        -- Sales are always processed chronologically:
        --
        --     SaleDate ASC
        --     SaleId   ASC
        --
        -- InitialOwnerId is the starting point and is never changed.
        --
        -- Coins.OwnerId becomes D.
        ------------------------------------------------------------

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
            --------------------------------------------------------
            -- The owner immediately before this Sale is the owner
            -- resulting from all preceding Sales.
            --------------------------------------------------------

            UPDATE [dbo].[Sales]
            SET [PreviousOwnerId] = @CurrentOwnerId
            WHERE [SaleId] = @CurrentSaleId;

            --------------------------------------------------------
            -- This Sale transfers ownership to its Buyer.
            --------------------------------------------------------

            SET @CurrentOwnerId = @CurrentBuyerId;

            FETCH NEXT FROM SalesCursor
            INTO @CurrentSaleId, @CurrentBuyerId;
        END;

        CLOSE SalesCursor;
        DEALLOCATE SalesCursor;

        ------------------------------------------------------------
        -- Update the current owner.
        --
        -- If Sales remain:
        --     OwnerId = BuyerId of the chronologically last Sale.
        --
        -- If no Sales remain:
        --     OwnerId = InitialOwnerId.
        --
        -- InitialOwnerId itself is never changed.
        ------------------------------------------------------------

        UPDATE [dbo].[Coins]
        SET [OwnerId] = @CurrentOwnerId
        WHERE [CoinId] = @CoinId;

        COMMIT TRANSACTION;

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