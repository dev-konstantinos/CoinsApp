CREATE PROCEDURE [dbo].[Countries_GetAll]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        [CountryId],
        [Name],
        [Code],
        [IsActive]
    FROM [dbo].[Countries]
    ORDER BY
        [Name],
        [CountryId];
END;