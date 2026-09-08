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

        ------------------------------------------------------------
        -- 1. Validate Coin and lock it
        --
        -- OwnerId may legitimately be NULL.
        -- Therefore Coin existence must be checked separately.
        ------------------------------------------------------------

        IF NOT EXISTS
        (
            SELECT 1
            FROM [dbo].[Coins] WITH (UPDLOCK, HOLDLOCK)
            WHERE [CoinId] = @CoinId
        )
        BEGIN
            RAISERROR ('Coin not found.', 16, 1);
            RETURN;
        END;

        SELECT
            @PreviousOwnerId = [OwnerId]
        FROM [dbo].[Coins] WITH (UPDLOCK, HOLDLOCK)
        WHERE [CoinId] = @CoinId;

        IF @SaleDate > SYSUTCDATETIME()
        BEGIN
            RAISERROR ('SaleDate cannot be in the future.', 16, 1);
            RETURN;
        END;

        IF @BuyerId <= 0
        BEGIN
            RAISERROR ('BuyerId must be greater than zero.', 16, 1);
            RETURN;
        END;
        ------------------------------------------------------------
        -- 2. Validate SaleDate
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
            WHERE [CoinId] = @CoinId;
        END;
        ------------------------------------------------------------
        -- 3. Create Sale
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
        -- 4. Transfer ownership
        --
        -- A NULL BuyerId means that the buyer is unknown.
        -- In that case the current OwnerId remains unchanged.
        ------------------------------------------------------------

        UPDATE [dbo].[Coins]
        SET [OwnerId] = @BuyerId
        WHERE [CoinId] = @CoinId;

        COMMIT TRANSACTION;
      ------------------------------------------------------------
        -- 5. Return new ID
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