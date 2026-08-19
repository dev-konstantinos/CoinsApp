CREATE TABLE [dbo].[CatalogEntries]
(
    [CatalogEntryId] INT IDENTITY(1,1) NOT NULL,
    [CatalogId] INT NOT NULL,
    [CoinId] INT NOT NULL,
    [CatalogNumber] NVARCHAR(50) NOT NULL,
    [Notes] NVARCHAR(1000) NULL,

    CONSTRAINT [PK_CatalogEntries]
        PRIMARY KEY ([CatalogEntryId]),

    CONSTRAINT [FK_CatalogEntries_Catalogs]
        FOREIGN KEY ([CatalogId])
        REFERENCES [dbo].[Catalogs] ([CatalogId]),

    CONSTRAINT [FK_CatalogEntries_Coins]
        FOREIGN KEY ([CoinId])
        REFERENCES [dbo].[Coins] ([CoinId]),

    CONSTRAINT [UQ_CatalogEntries_Catalog_Coin]
        UNIQUE ([CatalogId], [CoinId]),

    CONSTRAINT [UQ_CatalogEntries_Catalog_Number]
        UNIQUE ([CatalogId], [CatalogNumber])
);