CREATE PROCEDURE [dbo].[Mints_SetActive]
    @MintId INT,
    @IsActive BIT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE [dbo].[Mints]
    SET
        [IsActive] = @IsActive
    WHERE [MintId] = @MintId;

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
    WHERE m.[MintId] = @MintId;
END;