CREATE PROCEDURE [dbo].[Mints_GetAll]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        m.[MintId],
        m.[CountryId],
        c.[Name] AS [CountryName],
        c.[Code] AS [CountryCode],
        m.[Name],
        m.[Code],
        m.[City],
        m.[IsActive]
    FROM [dbo].[Mints] AS m
    INNER JOIN [dbo].[Countries] AS c
        ON c.[CountryId] = m.[CountryId]
    ORDER BY
        c.[Name],
        m.[Name],
        m.[MintId];
END;