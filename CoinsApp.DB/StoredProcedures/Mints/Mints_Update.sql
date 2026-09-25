CREATE PROCEDURE [dbo].[Mints_Update]
    @MintId INT,
    @CountryId INT,
    @Name NVARCHAR(100),
    @Code NVARCHAR(20),
    @City NVARCHAR(100),
    @IsActive BIT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE [dbo].[Mints]
    SET
        [CountryId] = @CountryId,
        [Name] = @Name,
        [Code] = @Code,
        [City] = @City,
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