CREATE PROCEDURE [dbo].[Collections_GetById]
    @CollectionId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        c.[CollectionId],
        c.[UserId],
        u.[UserName],
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
        u.[UserName],
        c.[Name],
        c.[Description],
        c.[IsActive],
        c.[CreatedAt];
END;
GO