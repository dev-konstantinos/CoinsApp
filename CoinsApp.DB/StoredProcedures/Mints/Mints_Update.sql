CREATE PROCEDURE [dbo].[Mints_Update]
    @MintId INT,
    @CountryId INT,
    @Name NVARCHAR(100),
    @Code NVARCHAR(20),
    @City NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE [dbo].[Mints]
    SET
        [CountryId] = @CountryId,
        [Name] = @Name,
        [Code] = @Code,
        [City] = @City
    WHERE [MintId] = @MintId;
END;