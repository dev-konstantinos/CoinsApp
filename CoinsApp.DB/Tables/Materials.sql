CREATE TABLE [dbo].[Materials]
(
    [MaterialId] INT IDENTITY(1,1) NOT NULL,

    [Name] NVARCHAR(100) NOT NULL,

    [Symbol] NVARCHAR(20) NULL,

    [IsPreciousMetal] BIT NOT NULL
        CONSTRAINT [DF_Materials_IsPreciousMetal]
        DEFAULT (0),

    [IsActive] BIT NOT NULL
        CONSTRAINT [DF_Materials_IsActive]
        DEFAULT (1),

    CONSTRAINT [PK_Materials]
        PRIMARY KEY ([MaterialId]),

    CONSTRAINT [UQ_Materials_Name]
        UNIQUE ([Name])
);