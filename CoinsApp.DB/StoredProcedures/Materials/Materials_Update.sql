CREATE PROCEDURE [dbo].[Materials_Update]
    @MaterialId INT,
    @Name NVARCHAR(100),
    @Symbol NVARCHAR(20) = NULL,
    @IsPreciousMetal BIT,
    @IsActive BIT
AS
BEGIN
    SET NOCOUNT ON;

    IF @MaterialId <= 0
        THROW 50034, 'Material ID must be greater than zero.', 1;

    SET @Name = LTRIM(RTRIM(@Name));
    SET @Symbol = NULLIF(LTRIM(RTRIM(@Symbol)), N'');

    IF @Name = N''
        THROW 50035, 'Material name is required.', 1;

    IF NOT EXISTS
    (
        SELECT 1
        FROM [dbo].[Materials]
        WHERE [MaterialId] = @MaterialId
    )
        THROW 50036, 'Material not found.', 1;

    IF EXISTS
    (
        SELECT 1
        FROM [dbo].[Materials]
        WHERE [Name] = @Name
          AND [MaterialId] <> @MaterialId
    )
        THROW 50037, 'Material name already exists.', 1;

    UPDATE [dbo].[Materials]
    SET
        [Name] = @Name,
        [Symbol] = @Symbol,
        [IsPreciousMetal] = @IsPreciousMetal,
        [IsActive] = @IsActive
    WHERE [MaterialId] = @MaterialId;

    SELECT
        [MaterialId],
        [Name],
        [Symbol],
        [IsPreciousMetal],
        [IsActive]
    FROM [dbo].[Materials]
    WHERE [MaterialId] = @MaterialId;
END;