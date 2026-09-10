CREATE PROCEDURE [dbo].[Collections_Create]
    @UserId INT,
    @Name NVARCHAR(150),
    @Description NVARCHAR(500) = NULL,
    @IsActive BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    IF @UserId <= 0
    BEGIN
        ROLLBACK TRANSACTION;
        RAISERROR('UserId must be greater than zero.', 16, 1);
        RETURN;
    END;

    IF @Name IS NULL OR LEN(LTRIM(RTRIM(@Name))) = 0
    BEGIN
        ROLLBACK TRANSACTION;
        RAISERROR('Collection name is required.', 16, 1);
        RETURN;
    END;

    SET @Name = LTRIM(RTRIM(@Name));

    IF NOT EXISTS
    (
        SELECT 1
        FROM [dbo].[Users]
        WHERE [UserId] = @UserId
    )
    BEGIN
        ROLLBACK TRANSACTION;
        RAISERROR('The specified user does not exist.', 16, 1);
        RETURN;
    END;

    IF EXISTS
    (
        SELECT 1
        FROM [dbo].[Collections]
        WHERE [UserId] = @UserId
          AND [Name] = @Name
    )
    BEGIN
        ROLLBACK TRANSACTION;
        RAISERROR('A collection with this name already exists for the specified user.', 16, 1);
        RETURN;
    END;

    INSERT INTO [dbo].[Collections]
    (
        [UserId],
        [Name],
        [Description],
        [IsActive]
    )
    VALUES
    (
        @UserId,
        @Name,
        @Description,
        @IsActive
    );

    SELECT
        CAST(SCOPE_IDENTITY() AS INT) AS [CollectionId];
END;
GO