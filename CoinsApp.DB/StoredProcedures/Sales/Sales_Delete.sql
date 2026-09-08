﻿CREATE PROCEDURE [dbo].[Sales_Delete]
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
        -- 1. Get Sale and lock it
        ------------------------------------------------------------

        SELECT
            @CoinId = [CoinId]
        FROM [dbo].[Sales] WITH (UPDLOCK, HOLDLOCK)
        WHERE [SaleId] = @SaleId;

        IF @@ROWCOUNT = 0
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
        -- 3. Determine original owner BEFORE deleting the Sale
        ------------------------------------------------------------

        SELECT TOP (1)
            @InitialOwnerId = [PreviousOwnerId]
        FROM [dbo].[Sales] WITH (UPDLOCK, HOLDLOCK)
        WHERE [CoinId] = @CoinId
        ORDER BY
            [SaleDate] ASC,
            [SaleId] ASC;


        ------------------------------------------------------------
        -- 4. Delete Sale
        ------------------------------------------------------------

        DELETE FROM [dbo].[Sales]
        WHERE [SaleId] = @SaleId;


        IF @@ROWCOUNT = 0
        BEGIN
            RAISERROR(
                'Sale could not be deleted.',
                16,
                1
            );
            RETURN;
        END;


        ------------------------------------------------------------
        -- 5. Rebuild remaining Sales chain
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
            -- Owner before this Sale
            --------------------------------------------------------

            UPDATE [dbo].[Sales]
            SET [PreviousOwnerId] = @CurrentOwnerId
            WHERE [SaleId] = @CurrentSaleId;


            --------------------------------------------------------
            -- Apply ownership transfer
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
        -- 6. Update current Coin owner
        ------------------------------------------------------------

        UPDATE [dbo].[Coins]
        SET [OwnerId] = @CurrentOwnerId
        WHERE [CoinId] = @CoinId;


        ------------------------------------------------------------
        -- 7. Commit
        ------------------------------------------------------------

        COMMIT TRANSACTION;


        ------------------------------------------------------------
        -- 8. Return deleted SaleId
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
