CREATE PROCEDURE [dbo].[Countries_SetActive]
    @CountryId INT,
    @IsActive BIT
AS
BEGIN
    SET NOCOUNT ON;

    IF @CountryId <= 0
        THROW 50010, 'Country ID must be greater than zero.', 1;

    IF NOT EXISTS
    (
        SELECT 1
        FROM [dbo].[Countries]
        WHERE [CountryId] = @CountryId
    )
        THROW 50011, 'Country not found.', 1;

    UPDATE [dbo].[Countries]
    SET
        [IsActive] = @IsActive
    WHERE [CountryId] = @CountryId;

    SELECT
        [CountryId],
        [Name],
        [Code],
        [IsActive]
    FROM [dbo].[Countries]
    WHERE [CountryId] = @CountryId;
END;