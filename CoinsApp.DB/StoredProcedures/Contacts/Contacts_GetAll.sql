CREATE PROCEDURE [dbo].[Contacts_GetAll]
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
    ORDER BY
        [Name] ASC,
        [ContactId] ASC;
END;
GO