CREATE PROCEDURE [dbo].[Contacts_Delete]
    @ContactId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    ------------------------------------------------------------
    -- Verify that the Contact exists.
    ------------------------------------------------------------

    IF NOT EXISTS
    (
        SELECT 1
        FROM [dbo].[Contacts]
        WHERE [ContactId] = @ContactId
    )
    BEGIN
        RAISERROR('Contact not found.', 16, 1);
        RETURN;
    END;

    ------------------------------------------------------------
    -- Delete the Contact.
    --
    -- Existing foreign keys protect Contacts that are still
    -- referenced by Coins, Purchases or Sales.
    ------------------------------------------------------------

    DELETE FROM [dbo].[Contacts]
    WHERE [ContactId] = @ContactId;
END;
GO