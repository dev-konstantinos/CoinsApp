CREATE PROCEDURE [dbo].[Materials_GetAll]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        [MaterialId],
        [Name],
        [Symbol],
        [IsPreciousMetal],
        [IsActive]
    FROM [dbo].[Materials]
    ORDER BY
        [Name],
        [MaterialId];
END;
