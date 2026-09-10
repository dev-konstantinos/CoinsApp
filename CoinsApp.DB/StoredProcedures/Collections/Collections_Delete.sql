CREATE PROCEDURE [dbo].[Collections_Delete]
    @CollectionId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF @CollectionId <= 0
    BEGIN
        ROLLBACK TRANSACTION;
        RAISERROR('CollectionId must be greater than zero.', 16, 1);
        RETURN;
    END;

    IF NOT EXISTS
    (
        SELECT 1
        FROM [dbo].[Collections]
        WHERE [CollectionId] = @CollectionId
    )
    BEGIN
        ROLLBACK TRANSACTION;
        RAISERROR('The specified collection does not exist.', 16, 1);
        RETURN;
    END;

    IF EXISTS
    (
        SELECT 1
        FROM [dbo].[Coins]
        WHERE [CollectionId] = @CollectionId
    )
    BEGIN
        ROLLBACK TRANSACTION;
        RAISERROR('The collection cannot be deleted because it still contains coins. Move or delete the coins first.', 16, 1);
        RETURN;
    END;

    DELETE FROM [dbo].[Collections]
    WHERE [CollectionId] = @CollectionId;

    SELECT
        @CollectionId AS [CollectionId];
END;
GO