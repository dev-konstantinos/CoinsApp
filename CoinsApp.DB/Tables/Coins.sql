CREATE TABLE [dbo].[Coins]
(
    [CoinId] INT IDENTITY(1,1) NOT NULL,
    [OwnerId] INT NULL,
    [CollectionId] INT NOT NULL,
    [CountryId] INT NOT NULL,
    [CurrencyId] INT NOT NULL,
    [DenominationId] INT NOT NULL,
    [MintId] INT NULL,
    [MaterialId] INT NULL,

    [Year] SMALLINT NULL,
    [MintMark] NVARCHAR(10) NULL,

    [Fineness] DECIMAL(7,4) NULL,
    [Weight] DECIMAL(12,4) NULL,
    [Diameter] DECIMAL(10,4) NULL,
    [Thickness] DECIMAL(10,4) NULL,

    [Shape] NVARCHAR(50) NULL,

    [Description] NVARCHAR(2000) NULL,

    [Designer] NVARCHAR(200) NULL,
    [Mintage] BIGINT NULL,

    [Condition] NVARCHAR(50) NULL,
    [Grade] NVARCHAR(20) NULL,
    [GradingCompany] NVARCHAR(100) NULL,
    [GradingCertificateNumber] NVARCHAR(100) NULL,

    [CurrentPrice] DECIMAL(19,4) NULL,
    [CurrentPriceCurrencyId] INT NULL,
    [CurrentPriceDate] DATETIME2(0) NULL,

    [Notes] NVARCHAR(2000) NULL,

    [CreatedAt] DATETIME2(0) NOT NULL
        CONSTRAINT [DF_Coins_CreatedAt]
        DEFAULT (SYSUTCDATETIME()),

    CONSTRAINT [PK_Coins]
        PRIMARY KEY ([CoinId]),

    CONSTRAINT [FK_Coins_Owner]
        FOREIGN KEY ([OwnerId])
        REFERENCES [dbo].[Contacts] ([ContactId]),

    CONSTRAINT [FK_Coins_Collections]
        FOREIGN KEY ([CollectionId])
        REFERENCES [dbo].[Collections] ([CollectionId]),

    CONSTRAINT [FK_Coins_Countries]
        FOREIGN KEY ([CountryId])
        REFERENCES [dbo].[Countries] ([CountryId]),

    CONSTRAINT [FK_Coins_Currencies]
        FOREIGN KEY ([CurrencyId])
        REFERENCES [dbo].[Currencies] ([CurrencyId]),

    CONSTRAINT [FK_Coins_Denominations]
        FOREIGN KEY ([CurrencyId], [DenominationId])
        REFERENCES [dbo].[Denominations]
        (
            [CurrencyId],
            [DenominationId]
        ),

    CONSTRAINT [FK_Coins_Mints]
        FOREIGN KEY ([CountryId], [MintId])
        REFERENCES [dbo].[Mints]
        (
            [CountryId],
            [MintId]
        ),

    CONSTRAINT [FK_Coins_Materials]
        FOREIGN KEY ([MaterialId])
        REFERENCES [dbo].[Materials] ([MaterialId]),

    CONSTRAINT [FK_Coins_CurrentPriceCurrency]
        FOREIGN KEY ([CurrentPriceCurrencyId])
        REFERENCES [dbo].[Currencies] ([CurrencyId]),
    
    CONSTRAINT [FK_Coins_CountryCurrencies]
    FOREIGN KEY ([CountryId], [CurrencyId])
    REFERENCES [dbo].[CountryCurrencies]
    (
        [CountryId],
        [CurrencyId]
    ),

    CONSTRAINT [CK_Coins_Fineness]
        CHECK
        (
            [Fineness] IS NULL
            OR ([Fineness] >= 0 AND [Fineness] <= 1000)
        ),

    CONSTRAINT [CK_Coins_Weight]
        CHECK
        (
            [Weight] IS NULL
            OR [Weight] > 0
        ),

    CONSTRAINT [CK_Coins_Diameter]
        CHECK
        (
            [Diameter] IS NULL
            OR [Diameter] > 0
        ),

    CONSTRAINT [CK_Coins_Thickness]
        CHECK
        (
            [Thickness] IS NULL
            OR [Thickness] > 0
        ),

    CONSTRAINT [CK_Coins_Mintage]
        CHECK
        (
            [Mintage] IS NULL
            OR [Mintage] >= 0
        ),

    CONSTRAINT [CK_Coins_CurrentPrice]
        CHECK
        (
            [CurrentPrice] IS NULL
            OR
            (
                [CurrentPrice] >= 0
                AND [CurrentPriceCurrencyId] IS NOT NULL
                AND [CurrentPriceDate] IS NOT NULL
            )
        )
);