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
    DECLARE @SaleId INT;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF NOT EXISTS
        (
            SELECT 1
            FROM [dbo].[Coins] WITH (UPDLOCK, HOLDLOCK)
            WHERE [CoinId] = @CoinId
        )
        BEGIN
            ROLLBACK TRANSACTION;
            RAISERROR ('Coin not found.', 16, 1);
            RETURN;
        END;

        SELECT
            @PreviousOwnerId = [OwnerId]
        FROM [dbo].[Coins] WITH (UPDLOCK, HOLDLOCK)
        WHERE [CoinId] = @CoinId;

        IF @SaleDate > SYSUTCDATETIME()
        BEGIN
            ROLLBACK TRANSACTION;
            RAISERROR ('SaleDate cannot be in the future.', 16, 1);
            RETURN;
        END;

        IF @BuyerId <= 0
        BEGIN
            ROLLBACK TRANSACTION;
            RAISERROR ('BuyerId must be greater than zero.', 16, 1);
            RETURN;
        END;

        ------------------------------------------------------------
        -- Preserve the original owner only once.
        ------------------------------------------------------------

        IF NOT EXISTS
        (
            SELECT 1
            FROM [dbo].[Sales] WITH (UPDLOCK, HOLDLOCK)
            WHERE [CoinId] = @CoinId
        )
        BEGIN
            UPDATE [dbo].[Coins]
            SET [InitialOwnerId] = [OwnerId]
            WHERE [CoinId] = @CoinId
              AND [InitialOwnerId] IS NULL;
        END;

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
        -- Transfer ownership to the buyer.
        ------------------------------------------------------------

        UPDATE [dbo].[Coins]
        SET [OwnerId] = @BuyerId
        WHERE [CoinId] = @CoinId;

        COMMIT TRANSACTION;

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