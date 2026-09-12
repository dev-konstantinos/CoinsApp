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
        RAISERROR('CollectionId must be greater than zero.', 16, 1);
        RETURN;
    END;

    IF @Name IS NULL OR LEN(LTRIM(RTRIM(@Name))) = 0
    BEGIN
        RAISERROR('Collection name is required.', 16, 1);
        RETURN;
    END;

    SET @Name = LTRIM(RTRIM(@Name));

    DECLARE @UserId INT;

    SELECT
        @UserId = [UserId]
    FROM [dbo].[Collections]
    WHERE [CollectionId] = @CollectionId;

    IF @UserId IS NULL
    BEGIN
        RAISERROR('The specified collection does not exist.', 16, 1);
        RETURN;
    END;

    IF EXISTS
    (
        SELECT 1
        FROM [dbo].[Collections]
        WHERE [UserId] = @UserId
          AND [Name] = @Name
          AND [CollectionId] <> @CollectionId
    )
    BEGIN
        RAISERROR(
            'A collection with this name already exists for the specified user.',
            16,
            1
        );
        RETURN;
    END;

    UPDATE [dbo].[Collections]
    SET
        [Name] = @Name,
        [Description] = @Description,
        [IsActive] = @IsActive
    WHERE [CollectionId] = @CollectionId;

    SELECT
        c.[CollectionId],
        c.[UserId],
        u.[Username] AS [UserName],
        c.[Name],
        c.[Description],
        c.[IsActive],
        c.[CreatedAt],
        COUNT(co.[CoinId]) AS [CoinCount]
    FROM [dbo].[Collections] AS c
    INNER JOIN [dbo].[Users] AS u
        ON u.[UserId] = c.[UserId]
    LEFT JOIN [dbo].[Coins] AS co
        ON co.[CollectionId] = c.[CollectionId]
    WHERE c.[CollectionId] = @CollectionId
    GROUP BY
        c.[CollectionId],
        c.[UserId],
        u.[Username],
        c.[Name],
        c.[Description],
        c.[IsActive],
        c.[CreatedAt];
END;
GO