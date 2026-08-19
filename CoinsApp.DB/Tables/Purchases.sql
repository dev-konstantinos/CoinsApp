CREATE TABLE [dbo].[Purchases]
(
    [PurchaseId] INT IDENTITY(1,1) NOT NULL,
    [CoinId] INT NOT NULL,
    [PurchaseDate] DATETIME2(0) NOT NULL,
    [PurchasePrice] DECIMAL(19,4) NOT NULL,
    [CurrencyId] INT NOT NULL,
    [SellerId] INT NULL,
    [Notes] NVARCHAR(2000) NULL,

    CONSTRAINT [PK_Purchases]
        PRIMARY KEY ([PurchaseId]),

    CONSTRAINT [FK_Purchases_Coins]
        FOREIGN KEY ([CoinId])
        REFERENCES [dbo].[Coins] ([CoinId]),

    CONSTRAINT [FK_Purchases_Currencies]
        FOREIGN KEY ([CurrencyId])
        REFERENCES [dbo].[Currencies] ([CurrencyId]),

    CONSTRAINT [FK_Purchases_Sellers]
        FOREIGN KEY ([SellerId])
        REFERENCES [dbo].[Contacts] ([ContactId]),

    CONSTRAINT [CK_Purchases_Price]
        CHECK ([PurchasePrice] >= 0)
);