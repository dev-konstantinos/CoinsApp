CREATE TABLE [dbo].[Users]
(
    [UserId] INT IDENTITY(1,1) NOT NULL,
    [Username] NVARCHAR(50) NOT NULL,
    [PasswordHash] NVARCHAR(500) NOT NULL,
    [Email] NVARCHAR(255) NULL,
    [IsActive] BIT NOT NULL
        CONSTRAINT [DF_Users_IsActive] DEFAULT (1),
    [CreatedAt] DATETIME2(0) NOT NULL
        CONSTRAINT [DF_Users_CreatedAt] DEFAULT (SYSUTCDATETIME()),

    CONSTRAINT [PK_Users]
        PRIMARY KEY ([UserId]),

    CONSTRAINT [UQ_Users_Username]
        UNIQUE ([Username]),

    CONSTRAINT [UQ_Users_Email]
        UNIQUE ([Email])
);