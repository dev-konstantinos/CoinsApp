CREATE PROCEDURE [dbo].[Materials_Create]
    @Name NVARCHAR(100),
    @Symbol NVARCHAR(20) = NULL,
    @IsPreciousMetal BIT = 0
AS
BEGIN
    SET NOCOUNT ON;

    SET @Name = LTRIM(RTRIM(@Name));
    SET @Symbol = NULLIF(LTRIM(RTRIM(@Symbol)), N'');

    IF @Name = N''
        THROW 50031, 'Material name is required.', 1;

    IF EXISTS
    (
        SELECT 1
        FROM [dbo].[Materials]
        WHERE [Name] = @Name
    )
        THROW 50032, 'Material name already exists.', 1;

    INSERT INTO [dbo].[Materials]
    (
        [Name],
        [Symbol],
        [IsPreciousMetal]
    )
    VALUES
    (
        @Name,
        @Symbol,
        @IsPreciousMetal
    );

    SELECT
        [MaterialId],
        [Name],
        [Symbol],
        [IsPreciousMetal]
    FROM [dbo].[Materials]
    WHERE [MaterialId] = SCOPE_IDENTITY();
END;
