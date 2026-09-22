CREATE PROCEDURE [dbo].[Currencies_Create]
    @Name NVARCHAR(100),
    @Code NVARCHAR(10),
    @Symbol NVARCHAR(10) = NULL,
    @IsActive BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    SET @Name = LTRIM(RTRIM(@Name));
    SET @Code = LTRIM(RTRIM(@Code));
    SET @Symbol = NULLIF(LTRIM(RTRIM(@Symbol)), N'');

    IF @Name = N''
        THROW 50020, 'Currency name is required.', 1;

    IF @Code = N''
        THROW 50021, 'Currency code is required.', 1;

    IF EXISTS
    (
        SELECT 1
        FROM [dbo].[Currencies]
        WHERE [Code] = @Code
    )
        THROW 50022, 'Currency code already exists.', 1;

    INSERT INTO [dbo].[Currencies]
    (
        [Name],
        [Code],
        [Symbol],
        [IsActive]
    )
    VALUES
    (
        @Name,
        @Code,
        @Symbol,
        @IsActive
    );

    SELECT
        [CurrencyId],
        [Name],
        [Code],
        [Symbol],
        [IsActive]
    FROM [dbo].[Currencies]
    WHERE [CurrencyId] = SCOPE_IDENTITY();
END;