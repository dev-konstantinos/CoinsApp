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

    BEGIN TRY
        BEGIN TRANSACTION;

        ------------------------------------------------------------
        -- 1. Validate SaleDate
        ------------------------------------------------------------

        IF @SaleDate > SYSUTCDATETIME()
        BEGIN
            IF XACT_STATE() <> 0
            BEGIN
                ROLLBACK TRANSACTION;
            END;

            RAISERROR ('SaleDate cannot be in the future.', 16, 1);
            RETURN;
        END;


        ------------------------------------------------------------
        -- 2. Update Sale
        ------------------------------------------------------------

        UPDATE [dbo].[Sales]
        SET
            [SaleDate] = @SaleDate,
            [SalePrice] = @SalePrice,
            [CurrencyId] = @CurrencyId,
            [BuyerId] = @BuyerId,
            [Notes] = @Notes
        WHERE [SaleId] = @SaleId;

        IF @@ROWCOUNT = 0
        BEGIN
            IF XACT_STATE() <> 0
            BEGIN
                ROLLBACK TRANSACTION;
            END;

            RAISERROR ('Sale not found.', 16, 1);
            RETURN;
        END;


        ------------------------------------------------------------
        -- 3. Commit
        --
        -- Sale does not modify:
        --   Coins.CurrentPrice
        --   PriceHistory
        ------------------------------------------------------------

        COMMIT TRANSACTION;


        ------------------------------------------------------------
        -- 4. Return updated ID
        ------------------------------------------------------------

        SELECT
            [SaleId]
        FROM [dbo].[Sales]
        WHERE [SaleId] = @SaleId;

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