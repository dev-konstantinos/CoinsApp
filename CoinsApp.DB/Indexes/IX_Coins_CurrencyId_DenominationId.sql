CREATE INDEX [IX_Coins_CurrencyId_DenominationId]
ON [dbo].[Coins]
(
    [CurrencyId],
    [DenominationId]
);