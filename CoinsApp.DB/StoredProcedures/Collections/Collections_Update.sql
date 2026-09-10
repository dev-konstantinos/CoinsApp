CREATE PROCEDURE [dbo].[Collections_Update]
    @CollectionId INT,
    @Name NVARCHAR(150),
    @Description NVARCHAR(500) = NULL,
    @IsActive BIT
AS
BEGIN
    SET NOCOUNT ON;

    IF @CollectionId <= 0
    BEGIN
        ROLLBACK TRANSACTION;
        RAISERROR('CollectionId must be greater than zero.', 16, 1);
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
        FROM [dbo].[Collections]
        WHERE [UserId] = 
        (
            SELECT [UserId]
            FROM [dbo].[Collections]
            WHERE [CollectionId] = @CollectionId
        )
          AND [Name] = @Name
          AND [CollectionId] <> @CollectionId
    )
    BEGIN
        ROLLBACK TRANSACTION;
        RAISERROR('A collection with this name already exists for the specified user.', 16, 1);
        RETURN;
    END;

    UPDATE [dbo].[Collections]
    SET
        [Name] = @Name,
        [Description] = @Description,
        [IsActive] = @IsActive
    WHERE [CollectionId] = @CollectionId;

    SELECT
        [CollectionId],
        [UserId],
        [Name],
        [Description],
        [IsActive],
        [CreatedAt]
    FROM [dbo].[Collections]
    WHERE [CollectionId] = @CollectionId;
END;
GO