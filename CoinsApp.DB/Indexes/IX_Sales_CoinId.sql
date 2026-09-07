CREATE INDEX [IX_Sales_CoinId_SaleDate]
ON [dbo].[Sales]
(
    [CoinId],
    [SaleDate] DESC,
    [SaleId] DESC
);