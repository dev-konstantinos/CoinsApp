CREATE PROCEDURE [dbo].[Materials_SetActive]
    @MaterialId INT,
    @IsActive BIT
AS
BEGIN
    SET NOCOUNT ON;

    IF @MaterialId <= 0
        THROW 50038, 'Material ID must be greater than zero.', 1;

    IF NOT EXISTS
    (
        SELECT 1
        FROM [dbo].[Materials]
        WHERE [MaterialId] = @MaterialId
    )
        THROW 50039, 'Material not found.', 1;

    UPDATE [dbo].[Materials]
    SET
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