CREATE TABLE [dbo].[CoinImages]
(
    [CoinImageId] INT IDENTITY(1,1) NOT NULL,
    [CoinId] INT NOT NULL,

    [ImageType] NVARCHAR(30) NOT NULL,
    [FileName] NVARCHAR(255) NOT NULL,
    [FilePath] NVARCHAR(1000) NOT NULL,

    [Description] NVARCHAR(500) NULL,
    [SortOrder] INT NOT NULL
        CONSTRAINT [DF_CoinImages_SortOrder] DEFAULT (0),

    [CreatedAt] DATETIME2(0) NOT NULL
        CONSTRAINT [DF_CoinImages_CreatedAt] DEFAULT (SYSUTCDATETIME()),

    CONSTRAINT [PK_CoinImages]
        PRIMARY KEY ([CoinImageId]),

    CONSTRAINT [FK_CoinImages_Coins]
        FOREIGN KEY ([CoinId])
        REFERENCES [dbo].[Coins] ([CoinId]),

    CONSTRAINT [CK_CoinImages_SortOrder]
        CHECK ([SortOrder] >= 0)
);