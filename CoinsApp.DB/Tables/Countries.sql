CREATE TABLE [dbo].[Countries]
(
    [CountryId] INT IDENTITY(1,1) NOT NULL,
    [Name] NVARCHAR(100) NOT NULL,
    [Code] NVARCHAR(10) NOT NULL,
    [IsActive] BIT NOT NULL
        CONSTRAINT [DF_Countries_IsActive] DEFAULT (1),

    CONSTRAINT [PK_Countries]
        PRIMARY KEY CLUSTERED ([CountryId]),

    CONSTRAINT [UQ_Countries_Code]
        UNIQUE ([Code])
);