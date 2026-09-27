CREATE PROCEDURE [dbo].[Users_Delete]
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    ------------------------------------------------------------
    -- Validate UserId.
    ------------------------------------------------------------

    IF @UserId IS NULL OR @UserId <= 0
    BEGIN
        RAISERROR('UserId must be greater than zero.', 16, 1);
        RETURN;
    END;

    ------------------------------------------------------------
    -- Verify that the User exists.
    ------------------------------------------------------------

    IF NOT EXISTS
    (
        SELECT 1
        FROM [dbo].[Users]
        WHERE [UserId] = @UserId
    )
    BEGIN
        RAISERROR('User not found.', 16, 1);
        RETURN;
    END;

    ------------------------------------------------------------
    -- Prevent deletion while Collections still reference
    -- this User.
    ------------------------------------------------------------

    IF EXISTS
    (
        SELECT 1
        FROM [dbo].[Collections]
        WHERE [UserId] = @UserId
    )
    BEGIN
        RAISERROR(
            'The user cannot be deleted because collections still reference this user.',
            16,
            1
        );
        RETURN;
    END;

    ------------------------------------------------------------
    -- Delete the User.
    ------------------------------------------------------------

    DELETE FROM [dbo].[Users]
    WHERE [UserId] = @UserId;

    ------------------------------------------------------------
    -- Return the deleted UserId.
    --
    -- UserRepository.DeleteAsync() expects a single INT.
    ------------------------------------------------------------

    SELECT
        @UserId AS [UserId];
END;
GO