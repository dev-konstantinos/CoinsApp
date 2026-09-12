CREATE PROCEDURE [dbo].[Sales_Update]
    @SaleId INT,
    @SaleDate DATETIME2(0),
    @SalePrice DECIMAL(19,4),
    @CurrencyId INT,
    @BuyerId INT,
    @Notes NVARCHAR(2000) = NULL
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
        -- All ownership-changing Sale operations use the Coin as
        -- the synchronization point. This prevents concurrent
        -- Sale Create / Update / Delete operations for the same Coin
        -- from rebuilding its ownership chain simultaneously.
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
        -- Validate the new Sale values.
        ------------------------------------------------------------

        IF @SaleDate > SYSUTCDATETIME()
        BEGIN
            ROLLBACK TRANSACTION;
            RAISERROR('SaleDate cannot be in the future.', 16, 1);
            RETURN;
        END;

        IF @BuyerId <= 0
        BEGIN
            ROLLBACK TRANSACTION;
            RAISERROR('BuyerId must be greater than zero.', 16, 1);
            RETURN;
        END;

        ------------------------------------------------------------
        -- InitialOwnerId is a historical value.
        --
        -- It is intentionally READ ONLY in Sales_Update.
        --
        -- Updating a Sale may change the chronological ownership
        -- chain, but it must never change the stored InitialOwnerId.
        ------------------------------------------------------------

        SELECT
            @InitialOwnerId = [InitialOwnerId]
        FROM [dbo].[Coins] WITH (UPDLOCK, HOLDLOCK)
        WHERE [CoinId] = @CoinId;

        ------------------------------------------------------------
        -- Update the Sale itself.
        --
        -- PreviousOwnerId is intentionally NOT updated here.
        -- It is recalculated below from the complete chronological
        -- ownership chain.
        ------------------------------------------------------------

        UPDATE [dbo].[Sales]
        SET
            [SaleDate] = @SaleDate,
            [SalePrice] = @SalePrice,
            [CurrencyId] = @CurrencyId,
            [BuyerId] = @BuyerId,
            [Notes] = @Notes
        WHERE [SaleId] = @SaleId;

        ------------------------------------------------------------
        -- Rebuild the complete ownership chain.
        --
        -- The chronological order is:
        --
        --     SaleDate ASC
        --     SaleId   ASC
        --
        -- SaleId is used as a deterministic tie-breaker when two
        -- Sales have the same SaleDate.
        --
        -- InitialOwnerId is only the starting point of the chain.
        -- It is NEVER modified by this rebuild.
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
            -- resulting from all chronologically preceding Sales.
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
        -- The Buyer of the chronologically last Sale is the current
        -- owner of the Coin.
        --
        -- If InitialOwnerId is NULL and Sales exist, the first Sale
        -- starts with an unknown PreviousOwnerId. That is valid.
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