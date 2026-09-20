CREATE PROCEDURE [dbo].[Countries_Create]
    @Name NVARCHAR(100),
    @Code NVARCHAR(10),
    @IsActive BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    SET @Name = LTRIM(RTRIM(@Name));
    SET @Code = LTRIM(RTRIM(@Code));

    IF @Name = N''
        THROW 50001, 'Country name is required.', 1;

    IF @Code = N''
        THROW 50002, 'Country code is required.', 1;

    IF EXISTS
    (
        SELECT 1
        FROM [dbo].[Countries]
        WHERE [Code] = @Code
    )
        THROW 50003, 'Country code already exists.', 1;

    INSERT INTO [dbo].[Countries]
    (
        [Name],
        [Code],
        [IsActive]
    )
    VALUES
    (
        @Name,
        @Code,
        @IsActive
    );

    SELECT
        [CountryId],
        [Name],
        [Code],
        [IsActive]
    FROM [dbo].[Countries]
    WHERE [CountryId] = SCOPE_IDENTITY();
END;