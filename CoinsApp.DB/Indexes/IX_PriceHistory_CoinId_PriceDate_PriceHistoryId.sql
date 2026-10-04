CREATE INDEX [IX_PriceHistory_CoinId_PriceDate_PriceHistoryId]
ON [dbo].[PriceHistory]
(
    [CoinId],
    [PriceDate] DESC,
    [PriceHistoryId] DESC
);
GO