CREATE PROCEDURE [dbo].[Currencies_Update]
    @CurrencyId INT,
    @Name NVARCHAR(100),
    @Code NVARCHAR(10),
    @Symbol NVARCHAR(10) = NULL,
    @IsActive BIT
AS
BEGIN
    SET NOCOUNT ON;

    IF @CurrencyId <= 0
        THROW 50024, 'Currency ID must be greater than zero.', 1;

    SET @Name = LTRIM(RTRIM(@Name));
    SET @Code = LTRIM(RTRIM(@Code));
    SET @Symbol = NULLIF(LTRIM(RTRIM(@Symbol)), N'');

    IF @Name = N''
        THROW 50025, 'Currency name is required.', 1;

    IF @Code = N''
        THROW 50026, 'Currency code is required.', 1;

    IF NOT EXISTS
    (
        SELECT 1
        FROM [dbo].[Currencies]
        WHERE [CurrencyId] = @CurrencyId
    )
        THROW 50027, 'Currency not found.', 1;

    IF EXISTS
    (
        SELECT 1
        FROM [dbo].[Currencies]
        WHERE [Code] = @Code
          AND [CurrencyId] <> @CurrencyId
    )
        THROW 50028, 'Currency code already exists.', 1;

    UPDATE [dbo].[Currencies]
    SET
        [Name] = @Name,
        [Code] = @Code,
        [Symbol] = @Symbol,
        [IsActive] = @IsActive
    WHERE [CurrencyId] = @CurrencyId;

    SELECT
        [CurrencyId],
        [Name],
        [Code],
        [Symbol],
        [IsActive]
    FROM [dbo].[Currencies]
    WHERE [CurrencyId] = @CurrencyId;
END;