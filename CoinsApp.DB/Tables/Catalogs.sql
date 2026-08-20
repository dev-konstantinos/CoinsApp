CREATE TABLE [dbo].[Catalogs]
(
    [CatalogId] INT IDENTITY(1,1) NOT NULL,
    [Name] NVARCHAR(150) NOT NULL,
    [ShortName] NVARCHAR(50) NULL,
    [Publisher] NVARCHAR(200) NULL,
    [Description] NVARCHAR(1000) NULL,
    [IsActive] BIT NOT NULL
        CONSTRAINT [DF_Catalogs_IsActive] DEFAULT (1),

    CONSTRAINT [PK_Catalogs]
        PRIMARY KEY ([CatalogId]),

    CONSTRAINT [UQ_Catalogs_Name]
        UNIQUE ([Name])
);