CREATE PROCEDURE [dbo].[Purchases_Delete]
    @PurchaseId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        DELETE FROM [dbo].[Purchases]
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

        COMMIT TRANSACTION;

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