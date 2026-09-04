CREATE PROCEDURE [dbo].[Purchases_Create]
    @CoinId INT,
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

        IF @PurchaseDate > SYSUTCDATETIME()
        BEGIN
            RAISERROR ('PurchaseDate cannot be in the future.', 16, 1);
            RETURN;
        END;

        INSERT INTO [dbo].[Purchases]
        (
            [CoinId],
            [PurchaseDate],
            [PurchasePrice],
            [CurrencyId],
            [SellerId],
            [Notes]
        )
        VALUES
        (
            @CoinId,
            @PurchaseDate,
            @PurchasePrice,
            @CurrencyId,
            @SellerId,
            @Notes
        );

        DECLARE @PurchaseId INT;

        SET @PurchaseId =
            CONVERT(INT, SCOPE_IDENTITY());

        COMMIT TRANSACTION;

        SELECT
            @PurchaseId AS [PurchaseId];

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