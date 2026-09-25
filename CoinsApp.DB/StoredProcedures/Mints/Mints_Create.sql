CREATE PROCEDURE [dbo].[Mints_Create]
    @CountryId INT,
    @Name NVARCHAR(100),
    @Code NVARCHAR(20),
    @City NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [dbo].[Mints]
    (
        [CountryId],
        [Name],
        [Code],
        [City],
        [IsActive]
    )
    VALUES
    (
        @CountryId,
        @Name,
        @Code,
        @City,
        1
    );

    DECLARE @MintId INT = CAST(SCOPE_IDENTITY() AS INT);

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