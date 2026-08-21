CREATE INDEX [IX_PriceHistory_CoinId_PriceDate]
ON [dbo].[PriceHistory]
(
    [CoinId],
    [PriceDate] DESC
);