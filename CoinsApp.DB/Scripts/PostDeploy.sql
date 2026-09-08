/*
    CoinsApp Post-Deployment Script

    Global seed only. Development test data is NOT executed here.
*/

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
-- Ownership migration
-- Preserve the original owner for existing Coins.
-- If Sales already exist, the first chronological Sale is
-- the authoritative historical starting point.
------------------------------------------------------------

UPDATE c
SET [InitialOwnerId] =
    CASE
        WHEN EXISTS
        (
            SELECT 1
            FROM [dbo].[Sales] s
            WHERE s.[CoinId] = c.[CoinId]
        )
        THEN
        (
            SELECT TOP (1)
                s.[PreviousOwnerId]
            FROM [dbo].[Sales] s
            WHERE s.[CoinId] = c.[CoinId]
            ORDER BY s.[SaleDate] ASC, s.[SaleId] ASC
        )
        ELSE c.[OwnerId]
    END
FROM [dbo].[Coins] c
WHERE c.[InitialOwnerId] IS NULL;


------------------------------------------------------------
-- Price migration
-- PriceHistory is the source of truth. Rebuild the materialized
-- current price for every existing coin.
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
