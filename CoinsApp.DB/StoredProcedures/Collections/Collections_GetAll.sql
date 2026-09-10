CREATE PROCEDURE [dbo].[Collections_GetAll]
    @UserId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

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
    WHERE @UserId IS NULL
       OR c.[UserId] = @UserId
    GROUP BY
        c.[CollectionId],
        c.[UserId],
        u.[Username],
        c.[Name],
        c.[Description],
        c.[IsActive],
        c.[CreatedAt]
    ORDER BY
        c.[Name] ASC,
        c.[CollectionId] ASC;
END;
GO