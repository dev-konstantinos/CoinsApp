CREATE PROCEDURE [dbo].[Sales_Update]
    @SaleId INT,
    @SaleDate DATETIME2(0),
    @SalePrice DECIMAL(19,4),
    @CurrencyId INT,
    @BuyerId INT = NULL,
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
        -- 1. Get Sale
        ------------------------------------------------------------

        SELECT
            @CoinId = [CoinId]
        FROM [dbo].[Sales] WITH (UPDLOCK, HOLDLOCK)
        WHERE [SaleId] = @SaleId;

        IF @CoinId IS NULL
        BEGIN
            RAISERROR('Sale not found.', 16, 1);
            RETURN;
        END;


        ------------------------------------------------------------
        -- 2. Lock Coin
        ------------------------------------------------------------

        IF NOT EXISTS
        (
            SELECT 1
            FROM [dbo].[Coins] WITH (UPDLOCK, HOLDLOCK)
            WHERE [CoinId] = @CoinId
        )
        BEGIN
            RAISERROR('Coin not found.', 16, 1);
            RETURN;
        END;


        ------------------------------------------------------------
        -- 3. Determine the original owner
        --
        -- The first chronological Sale contains the owner that
        -- existed before the Sales chain started.
        ------------------------------------------------------------

        SELECT TOP (1)
            @InitialOwnerId = [PreviousOwnerId]
        FROM [dbo].[Sales] WITH (UPDLOCK, HOLDLOCK)
        WHERE [CoinId] = @CoinId
        ORDER BY
            [SaleDate] ASC,
            [SaleId] ASC;


        ------------------------------------------------------------
        -- 4. Validate SaleDate
        ------------------------------------------------------------

        IF @SaleDate > SYSUTCDATETIME()
        BEGIN
            RAISERROR('SaleDate cannot be in the future.', 16, 1);
            RETURN;
        END;


        ------------------------------------------------------------
        -- 5. Update Sale
        --
        -- PreviousOwnerId is rebuilt below.
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
        -- 6. Rebuild the complete ownership chain
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
        INTO
            @CurrentSaleId,
            @CurrentBuyerId;

        WHILE @@FETCH_STATUS = 0
        BEGIN

            --------------------------------------------------------
            -- Owner immediately before this Sale
            --------------------------------------------------------

            UPDATE [dbo].[Sales]
            SET [PreviousOwnerId] = @CurrentOwnerId
            WHERE [SaleId] = @CurrentSaleId;


            --------------------------------------------------------
            -- Apply ownership transfer
            --
            -- NULL BuyerId means that the owner does not change.
            --------------------------------------------------------

            IF @CurrentBuyerId IS NOT NULL
            BEGIN
                SET @CurrentOwnerId = @CurrentBuyerId;
            END;


            FETCH NEXT FROM SalesCursor
            INTO
                @CurrentSaleId,
                @CurrentBuyerId;

        END;

        CLOSE SalesCursor;
        DEALLOCATE SalesCursor;


        ------------------------------------------------------------
        -- 7. Store the final owner on Coins
        ------------------------------------------------------------

        UPDATE [dbo].[Coins]
        SET [OwnerId] = @CurrentOwnerId
        WHERE [CoinId] = @CoinId;


        ------------------------------------------------------------
        -- 8. Commit
        ------------------------------------------------------------

        COMMIT TRANSACTION;


        ------------------------------------------------------------
        -- 9. Return SaleId
        ------------------------------------------------------------

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