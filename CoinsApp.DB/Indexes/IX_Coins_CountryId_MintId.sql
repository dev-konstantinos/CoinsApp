CREATE INDEX [IX_Coins_CountryId_MintId]
ON [dbo].[Coins]
(
    [CountryId],
    [MintId]
);