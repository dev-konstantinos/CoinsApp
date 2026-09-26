CREATE PROCEDURE [dbo].[Materials_GetById]
    @MaterialId INT
AS
BEGIN
    SET NOCOUNT ON;

    IF @MaterialId <= 0
        THROW 50033, 'Material ID must be greater than zero.', 1;

    SELECT
        [MaterialId],
        [Name],
        [Symbol],
        [IsPreciousMetal],
        [IsActive]
    FROM [dbo].[Materials]
    WHERE [MaterialId] = @MaterialId;
END;
