CREATE TABLE [dbo].[Contacts]
(
    [ContactId] INT IDENTITY(1,1) NOT NULL,
    [Name] NVARCHAR(150) NOT NULL,
    [CompanyName] NVARCHAR(200) NULL,
    [Email] NVARCHAR(255) NULL,
    [Phone] NVARCHAR(50) NULL,
    [Address] NVARCHAR(500) NULL,
    [Website] NVARCHAR(500) NULL,
    [Notes] NVARCHAR(2000) NULL,
    [IsActive] BIT NOT NULL
        CONSTRAINT [DF_Contacts_IsActive] DEFAULT (1),

    CONSTRAINT [PK_Contacts]
        PRIMARY KEY ([ContactId])
);