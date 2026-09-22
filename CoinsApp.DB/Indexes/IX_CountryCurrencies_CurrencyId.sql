CREATE NONCLUSTERED INDEX [IX_CountryCurrencies_CurrencyId]
ON [dbo].[CountryCurrencies]
(
    [CurrencyId],
    [CountryId]
);