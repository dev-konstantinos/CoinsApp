CREATE PROCEDURE [dbo].[Purchases_Update]
    @PurchaseId INT,
    @PurchaseDate DATETIME2(0),
    @PurchasePrice DECIMAL(19,4),
    @CurrencyId INT,
    @SellerId INT = NULL,
    @Notes NVARCHAR(2000) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        ------------------------------------------------------------
        -- 1. Validate PurchaseDate
        ------------------------------------------------------------

        IF @PurchaseDate > SYSUTCDATETIME()
        BEGIN
            IF XACT_STATE() <> 0
            BEGIN
                ROLLBACK TRANSACTION;
            END;

            RAISERROR ('PurchaseDate cannot be in the future.', 16, 1);
            RETURN;
        END;


        ------------------------------------------------------------
        -- 2. Update Purchase
        ------------------------------------------------------------

        UPDATE [dbo].[Purchases]
        SET
            [PurchaseDate] = @PurchaseDate,
            [PurchasePrice] = @PurchasePrice,
            [CurrencyId] = @CurrencyId,
            [SellerId] = @SellerId,
            [Notes] = @Notes
        WHERE [PurchaseId] = @PurchaseId;

        IF @@ROWCOUNT = 0
        BEGIN
            IF XACT_STATE() <> 0
            BEGIN
                ROLLBACK TRANSACTION;
            END;

            RAISERROR ('Purchase not found.', 16, 1);
            RETURN;
        END;


        ------------------------------------------------------------
        -- 3. Commit
        --
        -- Purchase does not modify:
        --   Coins.CurrentPrice
        --   PriceHistory
        ------------------------------------------------------------

        COMMIT TRANSACTION;


        ------------------------------------------------------------
        -- 4. Return updated ID
        ------------------------------------------------------------

        SELECT
            [PurchaseId]
        FROM [dbo].[Purchases]
        WHERE [PurchaseId] = @PurchaseId;

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