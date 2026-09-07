CREATE PROCEDURE [dbo].[Sales_Create]
    @CoinId INT,
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
        -- 1. Validate Coin
        ------------------------------------------------------------

        IF NOT EXISTS
        (
            SELECT 1
            FROM [dbo].[Coins]
            WHERE [CoinId] = @CoinId
        )
        BEGIN
            RAISERROR ('Coin not found.', 16, 1);
            RETURN;
        END;


        ------------------------------------------------------------
        -- 2. Validate SaleDate
        ------------------------------------------------------------

        IF @SaleDate > SYSUTCDATETIME()
        BEGIN
            RAISERROR ('SaleDate cannot be in the future.', 16, 1);
            RETURN;
        END;


        ------------------------------------------------------------
        -- 3. Insert Sale
        --
        -- Sale does not modify:
        --   Coins.CurrentPrice
        --   PriceHistory
        ------------------------------------------------------------

        INSERT INTO [dbo].[Sales]
        (
            [CoinId],
            [SaleDate],
            [SalePrice],
            [CurrencyId],
            [BuyerId],
            [Notes]
        )
        VALUES
        (
            @CoinId,
            @SaleDate,
            @SalePrice,
            @CurrencyId,
            @BuyerId,
            @Notes
        );


        DECLARE @SaleId INT;

        SET @SaleId =
            CONVERT(INT, SCOPE_IDENTITY());


        COMMIT TRANSACTION;


        ------------------------------------------------------------
        -- 4. Return new ID
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