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

    DECLARE @HasSales BIT = 0;

    BEGIN TRY
        BEGIN TRANSACTION;

        ------------------------------------------------------------
        -- Lock and verify the Coin.
        --
        -- The Coin is the synchronization point for ownership-
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
        -- Read the current ownership state.
        ------------------------------------------------------------

        SELECT
            @PreviousOwnerId = [OwnerId],
            @InitialOwnerId = [InitialOwnerId]
        FROM [dbo].[Coins] WITH (UPDLOCK, HOLDLOCK)
        WHERE [CoinId] = @CoinId;

        ------------------------------------------------------------
        -- Validate Sale data.
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
        -- Determine whether this Coin already has Sales.
        --
        -- This distinction is important:
        --
        -- 1. No previous Sales:
        --    If OwnerId is known and InitialOwnerId is NULL,
        --    OwnerId becomes the initial known owner.
        --
        -- 2. Previous Sales exist:
        --    InitialOwnerId must NOT be inferred from the current
        --    OwnerId. The new Sale may be entered with an earlier
        --    SaleDate, so the current OwnerId may belong to a later
        --    point in the ownership history.
        ------------------------------------------------------------

        IF EXISTS
        (
            SELECT 1
            FROM [dbo].[Sales]
            WHERE [CoinId] = @CoinId
        )
        BEGIN
            SET @HasSales = 1;
        END;

        ------------------------------------------------------------
        -- Establish InitialOwnerId only once, and only when this
        -- is the first Sale for the Coin.
        --
        -- If both InitialOwnerId and OwnerId are NULL, the original
        -- owner remains unknown.
        ------------------------------------------------------------

        IF @HasSales = 0
           AND @InitialOwnerId IS NULL
           AND @PreviousOwnerId IS NOT NULL
        BEGIN
            UPDATE [dbo].[Coins]
            SET [InitialOwnerId] = [OwnerId]
            WHERE [CoinId] = @CoinId
              AND [InitialOwnerId] IS NULL;

            SET @InitialOwnerId = @PreviousOwnerId;
        END;

        ------------------------------------------------------------
        -- Insert the Sale.
        --
        -- PreviousOwnerId is initially based on the current state.
        -- It is recalculated below after the complete chronological
        -- ownership chain has been rebuilt.
        ------------------------------------------------------------

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
        -- Read InitialOwnerId again after the possible first-owner
        -- assignment.
        --
        -- InitialOwnerId is the fixed starting point of the
        -- ownership chain. It is never recalculated from Sales.
        ------------------------------------------------------------

        SELECT
            @InitialOwnerId = [InitialOwnerId]
        FROM [dbo].[Coins] WITH (UPDLOCK, HOLDLOCK)
        WHERE [CoinId] = @CoinId;

        SET @CurrentOwnerId = @InitialOwnerId;

        ------------------------------------------------------------
        -- Rebuild the complete ownership chain chronologically.
        --
        -- SaleDate is the primary ordering criterion.
        -- SaleId provides deterministic ordering when two Sales have
        -- the same SaleDate.
        --
        -- This makes the final ownership state independent from the
        -- order in which Sales were entered.
        ------------------------------------------------------------

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
        -- Store the last chronological Buyer as the current owner.
        --
        -- If the ownership chain starts with NULL, the first
        -- PreviousOwnerId remains NULL. This is a valid state and
        -- means that the previous owner is unknown.
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