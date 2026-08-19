CREATE TABLE [dbo].[Currencies]
(
    [CurrencyId] INT IDENTITY(1,1) NOT NULL,
    [Code] NVARCHAR(10) NOT NULL,
    [Name] NVARCHAR(100) NOT NULL,
    [Symbol] NVARCHAR(10) NULL,
    [IsActive] BIT NOT NULL
        CONSTRAINT [DF_Currencies_IsActive] DEFAULT (1),

    CONSTRAINT [PK_Currencies]
        PRIMARY KEY ([CurrencyId]),

    CONSTRAINT [UQ_Currencies_Code]
        UNIQUE ([Code]),

    CONSTRAINT [UQ_Currencies_Name]
        UNIQUE ([Name])
);