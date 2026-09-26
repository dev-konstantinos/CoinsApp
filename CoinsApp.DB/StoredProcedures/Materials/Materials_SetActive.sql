CREATE PROCEDURE [dbo].[Materials_SetActive]
    @MaterialId INT,
    @IsActive BIT
AS
BEGIN
    SET NOCOUNT ON;

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