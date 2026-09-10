CREATE TABLE [dbo].[Collections]
(
    [CollectionId] INT IDENTITY(1,1) NOT NULL,
    [UserId] INT NOT NULL,
    [Name] NVARCHAR(150) NOT NULL,
    [Description] NVARCHAR(500) NULL,
    [IsActive] BIT NOT NULL
        CONSTRAINT [DF_Collections_IsActive] DEFAULT (1),
    [CreatedAt] DATETIME2(0) NOT NULL
        CONSTRAINT [DF_Collections_CreatedAt] DEFAULT (SYSUTCDATETIME()),

    CONSTRAINT [PK_Collections]
        PRIMARY KEY ([CollectionId]),

    CONSTRAINT [FK_Collections_Users]
        FOREIGN KEY ([UserId])
        REFERENCES [dbo].[Users] ([UserId]),

    CONSTRAINT [UQ_Collections_User_Name]
        UNIQUE ([UserId], [Name]),

    CONSTRAINT [CK_Collections_Name]
        CHECK (LEN(LTRIM(RTRIM([Name]))) > 0)
);