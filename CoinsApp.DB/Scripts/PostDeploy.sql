/*
    CoinsApp Post-Deployment Script

    Global reference data and materialized price synchronization.

    Ownership data is intentionally NOT modified here.
    InitialOwnerId is application/domain data and must not be
    recalculated during deployment.
*/

------------------------------------------------------------
-- Global reference data
------------------------------------------------------------

:r ..\Seed\Global\Countries.sql
GO

:r ..\Seed\Global\Currencies.sql
GO

:r ..\Seed\Global\CountryCurrencies.sql
GO

:r ..\Seed\Global\Materials.sql
GO

:r ..\Seed\Global\Mints.sql
GO

:r ..\Seed\Global\Denominations.sql
GO

:r ..\Seed\Global\Catalogs.sql
GO


------------------------------------------------------------
-- Price migration
--
-- PriceHistory is the source of truth.
--
-- Coins.CurrentPrice,
-- Coins.CurrentPriceCurrencyId and
-- Coins.CurrentPriceDate
-- are materialized values and are synchronized from the latest
-- PriceHistory record.
------------------------------------------------------------

UPDATE c
SET
    [CurrentPrice] = ph.[Price],
    [CurrentPriceCurrencyId] = ph.[CurrencyId],
    [CurrentPriceDate] = ph.[PriceDate]
FROM [dbo].[Coins] AS c
OUTER APPLY
(
    SELECT TOP (1)
        [Price],
        [CurrencyId],
        [PriceDate]
    FROM [dbo].[PriceHistory]
    WHERE [CoinId] = c.[CoinId]
    ORDER BY
        [PriceDate] DESC,
        [PriceHistoryId] DESC
) AS ph;


------------------------------------------------------------
-- Coins without PriceHistory must not retain stale
-- materialized price values.
------------------------------------------------------------

UPDATE c
SET
    [CurrentPrice] = NULL,
    [CurrentPriceCurrencyId] = NULL,
    [CurrentPriceDate] = NULL
FROM [dbo].[Coins] AS c
WHERE NOT EXISTS
(
    SELECT 1
    FROM [dbo].[PriceHistory] AS ph
    WHERE ph.[CoinId] = c.[CoinId]
);