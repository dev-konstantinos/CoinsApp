CREATE PROCEDURE [dbo].[Mints_SetActive]
    @MintId INT,
    @IsActive BIT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE [dbo].[Mints]
    SET
        [IsActive] = @IsActive
    WHERE [MintId] = @MintId;
END;