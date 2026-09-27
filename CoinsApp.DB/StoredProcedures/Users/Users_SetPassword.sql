CREATE PROCEDURE [dbo].[Users_SetPassword]
    @UserId INT,
    @PasswordHash NVARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    ------------------------------------------------------------
    -- Validate UserId.
    ------------------------------------------------------------

    IF @UserId IS NULL
    BEGIN
        RAISERROR('UserId is required.', 16, 1);
        RETURN;
    END;

    ------------------------------------------------------------
    -- Validate PasswordHash.
    --
    -- The BLL is responsible for hashing the plain-text password.
    -- This procedure must never receive a plain-text password.
    ------------------------------------------------------------

    IF @PasswordHash IS NULL
       OR LEN(LTRIM(RTRIM(@PasswordHash))) = 0
    BEGIN
        RAISERROR('PasswordHash is required.', 16, 1);
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
    -- Update only the password hash.
    ------------------------------------------------------------

    UPDATE [dbo].[Users]
    SET
        [PasswordHash] = @PasswordHash
    WHERE [UserId] = @UserId;
END;
GO