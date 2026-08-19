CREATE TABLE [dbo].[PriceHistory]
(
    [PriceHistoryId] INT IDENTITY(1,1) NOT NULL,
    [CoinId] INT NOT NULL,
    [Price] DECIMAL(19,4) NOT NULL,
    [CurrencyId] INT NOT NULL,
    [PriceDate] DATETIME2(0) NOT NULL,
    [Source] NVARCHAR(200) NULL,
    [Notes] NVARCHAR(1000) NULL,

    CONSTRAINT [PK_PriceHistory]
        PRIMARY KEY ([PriceHistoryId]),

    CONSTRAINT [FK_PriceHistory_Coins]
        FOREIGN KEY ([CoinId])
        REFERENCES [dbo].[Coins] ([CoinId]),

    CONSTRAINT [FK_PriceHistory_Currencies]
        FOREIGN KEY ([CurrencyId])
        REFERENCES [dbo].[Currencies] ([CurrencyId]),

    CONSTRAINT [CK_PriceHistory_Price]
        CHECK ([Price] >= 0)
);