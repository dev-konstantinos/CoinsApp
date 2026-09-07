CREATE PROCEDURE [dbo].[Sales_Delete]
    @SaleId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        DELETE FROM [dbo].[Sales]
        WHERE [SaleId] = @SaleId;

        IF @@ROWCOUNT = 0
        BEGIN
            RAISERROR ('Sale not found.', 16, 1);
            RETURN;
        END;


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