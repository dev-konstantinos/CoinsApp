CREATE INDEX [IX_Coins_CountryId_CurrencyId]
ON [dbo].[Coins]
(
    [CountryId],
    [CurrencyId]
);