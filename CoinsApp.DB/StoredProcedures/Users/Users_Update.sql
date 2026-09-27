CREATE PROCEDURE [dbo].[Users_Update]
    @UserId INT,
    @Username NVARCHAR(50),
    @Email NVARCHAR(255) = NULL,
    @IsActive BIT
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
    -- Validate Username.
    ------------------------------------------------------------

    IF @Username IS NULL
       OR LEN(LTRIM(RTRIM(@Username))) = 0
    BEGIN
        RAISERROR('Username is required.', 16, 1);
        RETURN;
    END;

    SET @Username = LTRIM(RTRIM(@Username));

    ------------------------------------------------------------
    -- Normalize optional Email.
    ------------------------------------------------------------

    SET @Email = NULLIF(LTRIM(RTRIM(@Email)), N'');

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
    -- Check Username uniqueness.
    ------------------------------------------------------------

    IF EXISTS
    (
        SELECT 1
        FROM [dbo].[Users]
        WHERE [Username] = @Username
          AND [UserId] <> @UserId
    )
    BEGIN
        RAISERROR('A user with this username already exists.', 16, 1);
        RETURN;
    END;

    ------------------------------------------------------------
    -- Check Email uniqueness.
    ------------------------------------------------------------

    IF @Email IS NOT NULL
       AND EXISTS
       (
           SELECT 1
           FROM [dbo].[Users]
           WHERE [Email] = @Email
             AND [UserId] <> @UserId
       )
    BEGIN
        RAISERROR('A user with this email already exists.', 16, 1);
        RETURN;
    END;

    ------------------------------------------------------------
    -- Update the User.
    --
    -- PasswordHash is intentionally NOT changed here.
    ------------------------------------------------------------

    UPDATE [dbo].[Users]
    SET
        [Username] = @Username,
        [Email] = @Email,
        [IsActive] = @IsActive
    WHERE [UserId] = @UserId;

    ------------------------------------------------------------
    -- Return the updated User.
    ------------------------------------------------------------

    SELECT
        u.[UserId],
        u.[Username],
        u.[Email],
        u.[IsActive],
        u.[CreatedAt],
        COUNT(c.[CollectionId]) AS [CollectionCount]
    FROM [dbo].[Users] AS u
    LEFT JOIN [dbo].[Collections] AS c
        ON c.[UserId] = u.[UserId]
    WHERE u.[UserId] = @UserId
    GROUP BY
        u.[UserId],
        u.[Username],
        u.[Email],
        u.[IsActive],
        u.[CreatedAt];
END;
GO