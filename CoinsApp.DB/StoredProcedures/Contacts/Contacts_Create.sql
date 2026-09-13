CREATE PROCEDURE [dbo].[Contacts_Create]
    @Name NVARCHAR(150),
    @CompanyName NVARCHAR(200) = NULL,
    @Email NVARCHAR(255) = NULL,
    @Phone NVARCHAR(50) = NULL,
    @Address NVARCHAR(500) = NULL,
    @Website NVARCHAR(500) = NULL,
    @Notes NVARCHAR(2000) = NULL,
    @IsActive BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    ------------------------------------------------------------
    -- Validate required data.
    ------------------------------------------------------------

    IF @Name IS NULL
       OR LEN(LTRIM(RTRIM(@Name))) = 0
    BEGIN
        RAISERROR('Contact name is required.', 16, 1);
        RETURN;
    END;

    ------------------------------------------------------------
    -- Normalize the contact name.
    ------------------------------------------------------------

    SET @Name = LTRIM(RTRIM(@Name));

    ------------------------------------------------------------
    -- Create the Contact.
    ------------------------------------------------------------

    INSERT INTO [dbo].[Contacts]
    (
        [Name],
        [CompanyName],
        [Email],
        [Phone],
        [Address],
        [Website],
        [Notes],
        [IsActive]
    )
    VALUES
    (
        @Name,
        @CompanyName,
        @Email,
        @Phone,
        @Address,
        @Website,
        @Notes,
        @IsActive
    );

    ------------------------------------------------------------
    -- Return the new ContactId.
    ------------------------------------------------------------

    SELECT
        CAST(SCOPE_IDENTITY() AS INT) AS [ContactId];
END;
GO