CREATE PROCEDURE [dbo].[Contacts_Update]
    @ContactId INT,
    @Name NVARCHAR(150),
    @CompanyName NVARCHAR(200) = NULL,
    @Email NVARCHAR(255) = NULL,
    @Phone NVARCHAR(50) = NULL,
    @Address NVARCHAR(500) = NULL,
    @Website NVARCHAR(500) = NULL,
    @Notes NVARCHAR(2000) = NULL,
    @IsActive BIT
AS
BEGIN
    SET NOCOUNT ON;

    ------------------------------------------------------------
    -- Validate ContactId.
    ------------------------------------------------------------

    IF @ContactId <= 0
    BEGIN
        RAISERROR('ContactId must be greater than zero.', 16, 1);
        RETURN;
    END;

    ------------------------------------------------------------
    -- Validate required data.
    ------------------------------------------------------------

    IF @Name IS NULL
       OR LEN(LTRIM(RTRIM(@Name))) = 0
    BEGIN
        RAISERROR('Contact name is required.', 16, 1);
        RETURN;
    END;

    SET @Name = LTRIM(RTRIM(@Name));

    ------------------------------------------------------------
    -- Update the Contact.
    ------------------------------------------------------------

    UPDATE [dbo].[Contacts]
    SET
        [Name] = @Name,
        [CompanyName] = @CompanyName,
        [Email] = @Email,
        [Phone] = @Phone,
        [Address] = @Address,
        [Website] = @Website,
        [Notes] = @Notes,
        [IsActive] = @IsActive
    WHERE [ContactId] = @ContactId;

    IF @@ROWCOUNT = 0
    BEGIN
        RAISERROR('Contact not found.', 16, 1);
        RETURN;
    END;

    ------------------------------------------------------------
    -- Return the updated ContactId.
    ------------------------------------------------------------

    SELECT
        [ContactId]
    FROM [dbo].[Contacts]
    WHERE [ContactId] = @ContactId;
END;
GO