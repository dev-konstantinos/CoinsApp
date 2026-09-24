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

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS [MintId];
END;