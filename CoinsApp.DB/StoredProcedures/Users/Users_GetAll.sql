CREATE PROCEDURE [dbo].[Users_GetAll]
AS
BEGIN
    SET NOCOUNT ON;

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
    GROUP BY
        u.[UserId],
        u.[Username],
        u.[Email],
        u.[IsActive],
        u.[CreatedAt]
    ORDER BY
        u.[UserId];
END;
GO