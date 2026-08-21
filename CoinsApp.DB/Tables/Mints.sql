CREATE TABLE [dbo].[Mints]
(
    [MintId] INT IDENTITY(1,1) NOT NULL,
    [CountryId] INT NOT NULL,
    [Name] NVARCHAR(150) NOT NULL,
    [Code] NVARCHAR(10) NULL,
    [City] NVARCHAR(100) NULL,
    [IsActive] BIT NOT NULL
        CONSTRAINT [DF_Mints_IsActive] DEFAULT (1),

    CONSTRAINT [PK_Mints]
        PRIMARY KEY ([MintId]),

    CONSTRAINT [FK_Mints_Countries]
        FOREIGN KEY ([CountryId])
        REFERENCES [dbo].[Countries] ([CountryId]),

    CONSTRAINT [UQ_Mints_Country_Name]
        UNIQUE ([CountryId], [Name])
);