CREATE INDEX [IX_Purchases_CoinId_PurchaseDate]
ON [dbo].[Purchases]
(
    [CoinId],
    [PurchaseDate] DESC,
    [PurchaseId] DESC
);