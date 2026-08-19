CREATE TABLE [dbo].[IssuingAuthorities]
(
    [IssuingAuthorityId] INT IDENTITY(1,1) NOT NULL,
    [CountryId] INT NOT NULL,
    [Name] NVARCHAR(150) NOT NULL,
    [Description] NVARCHAR(500) NULL,
    [IsActive] BIT NOT NULL
        CONSTRAINT [DF_IssuingAuthorities_IsActive] DEFAULT (1),

    CONSTRAINT [PK_IssuingAuthorities]
        PRIMARY KEY ([IssuingAuthorityId]),

    CONSTRAINT [FK_IssuingAuthorities_Countries]
        FOREIGN KEY ([CountryId])
        REFERENCES [dbo].[Countries] ([CountryId]),

    CONSTRAINT [UQ_IssuingAuthorities_Country_Name]
        UNIQUE ([CountryId], [Name])
);