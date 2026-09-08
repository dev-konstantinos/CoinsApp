CREATE PROCEDURE [dbo].[Sales_Delete]
    @SaleId INT
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
        -- Read and lock Sale
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
        -- Restore previous ownership
        ------------------------------------------------------------

        UPDATE [dbo].[Coins]
        SET [OwnerId] = @PreviousOwnerId
        WHERE [CoinId] = @CoinId;


        ------------------------------------------------------------
        -- Delete Sale
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
        -- Commit
        ------------------------------------------------------------

        COMMIT TRANSACTION;


        ------------------------------------------------------------
        -- Return deleted Sale ID
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