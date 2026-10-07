CREATE INDEX [IX_Purchases_CoinId_PurchaseDate_PurchaseId]
ON [dbo].[Purchases]
(
    [CoinId],
    [PurchaseDate] DESC,
    [PurchaseId] DESC
);
GO