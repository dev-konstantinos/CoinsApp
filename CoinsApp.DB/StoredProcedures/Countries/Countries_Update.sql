CREATE PROCEDURE [dbo].[Countries_Update]
    @CountryId INT,
    @Name NVARCHAR(100),
    @Code NVARCHAR(10),
    @IsActive BIT
AS
BEGIN
    SET NOCOUNT ON;

    IF @CountryId <= 0
        THROW 50005, 'Country ID must be greater than zero.', 1;

    SET @Name = LTRIM(RTRIM(@Name));
    SET @Code = LTRIM(RTRIM(@Code));

    IF @Name = N''
        THROW 50006, 'Country name is required.', 1;

    IF @Code = N''
        THROW 50007, 'Country code is required.', 1;

    IF NOT EXISTS
    (
        SELECT 1
        FROM [dbo].[Countries]
        WHERE [CountryId] = @CountryId
    )
        THROW 50008, 'Country not found.', 1;

    IF EXISTS
    (
        SELECT 1
        FROM [dbo].[Countries]
        WHERE [Code] = @Code
          AND [CountryId] <> @CountryId
    )
        THROW 50009, 'Country code already exists.', 1;

    UPDATE [dbo].[Countries]
    SET
        [Name] = @Name,
        [Code] = @Code,
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