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
    DECLARE @PreviousOwnerId INT;

    BEGIN TRY

        ------------------------------------------------------------
        -- Start transaction
        ------------------------------------------------------------

        BEGIN TRANSACTION;


        ------------------------------------------------------------
        -- Read and lock existing Sale
        ------------------------------------------------------------

        SELECT
            @CoinId = [CoinId],
            @PreviousOwnerId = [PreviousOwnerId]
        FROM [dbo].[Sales] WITH (UPDLOCK, HOLDLOCK)
        WHERE [SaleId] = @SaleId;


        IF @@ROWCOUNT = 0
        BEGIN
            RAISERROR(
                'Sale not found.',
                16,
                1
            );
            RETURN;
        END;


        ------------------------------------------------------------
        -- Validate SaleDate
        ------------------------------------------------------------

        IF @SaleDate > SYSUTCDATETIME()
        BEGIN
            RAISERROR(
                'SaleDate cannot be in the future.',
                16,
                1
            );
            RETURN;
        END;


        ------------------------------------------------------------
        -- Update Sale
        --
        -- PreviousOwnerId intentionally remains unchanged.
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
        -- Update current ownership
        --
        -- Buyer specified:
        --     Owner becomes Buyer.
        --
        -- Buyer NULL:
        --     Restore the owner that existed before the Sale.
        ------------------------------------------------------------

        UPDATE [dbo].[Coins]
        SET [OwnerId] =
            COALESCE(@BuyerId, @PreviousOwnerId)
        WHERE [CoinId] = @CoinId;


        ------------------------------------------------------------
        -- Commit
        ------------------------------------------------------------

        COMMIT TRANSACTION;


        ------------------------------------------------------------
        -- Return Sale ID
        ------------------------------------------------------------

        SELECT
            @SaleId AS [SaleId];

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