CREATE PROCEDURE [dbo].[Contacts_GetById]
    @ContactId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        [ContactId],
        [Name],
        [CompanyName],
        [Email],
        [Phone],
        [Address],
        [Website],
        [Notes],
        [IsActive]
    FROM [dbo].[Contacts]
    WHERE [ContactId] = @ContactId;
END;
GO