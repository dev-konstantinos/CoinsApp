CREATE TABLE [dbo].[CountryCurrencies]
(
    [CountryCurrencyId] INT IDENTITY(1,1) NOT NULL,
    [CountryId] INT NOT NULL,
    [CurrencyId] INT NOT NULL,
    [IsActive] BIT NOT NULL
        CONSTRAINT [DF_CountryCurrencies_IsActive] DEFAULT (1),

    CONSTRAINT [PK_CountryCurrencies]
        PRIMARY KEY CLUSTERED ([CountryCurrencyId]),

    CONSTRAINT [UQ_CountryCurrencies_Country_Currency]
        UNIQUE ([CountryId], [CurrencyId]),

    CONSTRAINT [FK_CountryCurrencies_Countries]
        FOREIGN KEY ([CountryId])
        REFERENCES [dbo].[Countries] ([CountryId]),

    CONSTRAINT [FK_CountryCurrencies_Currencies]
        FOREIGN KEY ([CurrencyId])
        REFERENCES [dbo].[Currencies] ([CurrencyId])
);