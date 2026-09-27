CREATE PROCEDURE [dbo].[Users_Create]
    @Username NVARCHAR(50),
    @PasswordHash NVARCHAR(500),
    @Email NVARCHAR(255) = NULL,
    @IsActive BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

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
    -- Validate PasswordHash.
    --
    -- The BLL is responsible for creating the hash.
    -- This procedure must never receive a plain-text password.
    ------------------------------------------------------------

    IF @PasswordHash IS NULL
       OR LEN(LTRIM(RTRIM(@PasswordHash))) = 0
    BEGIN
        RAISERROR('PasswordHash is required.', 16, 1);
        RETURN;
    END;

    ------------------------------------------------------------
    -- Normalize optional Email.
    ------------------------------------------------------------

    SET @Email = NULLIF(LTRIM(RTRIM(@Email)), N'');

    ------------------------------------------------------------
    -- Check Username uniqueness.
    --
    -- The UNIQUE constraint on Users.Username remains the final
    -- database-level protection against duplicates.
    ------------------------------------------------------------

    IF EXISTS
    (
        SELECT 1
        FROM [dbo].[Users]
        WHERE [Username] = @Username
    )
    BEGIN
        RAISERROR('A user with this username already exists.', 16, 1);
        RETURN;
    END;

    ------------------------------------------------------------
    -- Check Email uniqueness.
    --
    -- Email is optional, therefore NULL is allowed.
    -- The filtered unique index protects non-NULL values.
    ------------------------------------------------------------

    IF @Email IS NOT NULL
       AND EXISTS
       (
           SELECT 1
           FROM [dbo].[Users]
           WHERE [Email] = @Email
       )
    BEGIN
        RAISERROR('A user with this email already exists.', 16, 1);
        RETURN;
    END;

    ------------------------------------------------------------
    -- Create the User.
    ------------------------------------------------------------

    INSERT INTO [dbo].[Users]
    (
        [Username],
        [PasswordHash],
        [Email],
        [IsActive]
    )
    VALUES
    (
        @Username,
        @PasswordHash,
        @Email,
        @IsActive
    );

    ------------------------------------------------------------
    -- Return the new UserId.
    ------------------------------------------------------------

    SELECT
        CAST(SCOPE_IDENTITY() AS INT) AS [UserId];
END;
GO