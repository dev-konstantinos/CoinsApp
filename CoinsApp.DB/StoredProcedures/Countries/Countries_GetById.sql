CREATE PROCEDURE [dbo].[Countries_GetById]
    @CountryId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF @CountryId <= 0
        THROW 50004, 'Country ID must be greater than zero.', 1;

    SELECT
        [CountryId],
        [Name],
        [Code],
        [IsActive]
    FROM [dbo].[Countries]
    WHERE [CountryId] = @CountryId;
END;