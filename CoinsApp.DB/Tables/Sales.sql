CREATE TABLE [dbo].[Sales]
(
    [SaleId] INT IDENTITY(1,1) NOT NULL,
    [CoinId] INT NOT NULL,
    [SaleDate] DATETIME2(0) NOT NULL,
    [SalePrice] DECIMAL(19,4) NOT NULL,
    [CurrencyId] INT NOT NULL,
    [PreviousOwnerId] INT NULL,
    [BuyerId] INT NOT NULL,
    [Notes] NVARCHAR(2000) NULL,

    CONSTRAINT [PK_Sales]
        PRIMARY KEY ([SaleId]),

    CONSTRAINT [FK_Sales_Coins]
        FOREIGN KEY ([CoinId])
        REFERENCES [dbo].[Coins] ([CoinId]),

    CONSTRAINT [FK_Sales_Currencies]
        FOREIGN KEY ([CurrencyId])
        REFERENCES [dbo].[Currencies] ([CurrencyId]),
    
    CONSTRAINT [FK_Sales_PreviousOwner]
        FOREIGN KEY ([PreviousOwnerId])
        REFERENCES [dbo].[Contacts] ([ContactId]),

    CONSTRAINT [FK_Sales_Buyers]
        FOREIGN KEY ([BuyerId])
        REFERENCES [dbo].[Contacts] ([ContactId]),

    CONSTRAINT [CK_Sales_Price]
        CHECK ([SalePrice] >= 0)
);