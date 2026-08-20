/*
    CoinsApp
    Development Seed

    Development/test data only.
    This script is NOT executed by the normal PostDeploy.
*/

DECLARE @UserId INT;
DECLARE @CollectionId INT;

DECLARE @GermanyId INT;
DECLARE @RussiaId INT;

DECLARE @EURId INT;
DECLARE @DEMId INT;
DECLARE @RUBId INT;

DECLARE @EUR2Id INT;
DECLARE @DEM5Id INT;
DECLARE @RUB5Id INT;

DECLARE @BerlinMintId INT;
DECLARE @MoscowMintId INT;

DECLARE @SilverId INT;
DECLARE @CopperId INT;

DECLARE @FederalGermanyId INT;
DECLARE @SovietUnionId INT;

DECLARE @KMId INT;
DECLARE @JId INT;

DECLARE @Coin2EuroId INT;
DECLARE @Coin5DEMId INT;
DECLARE @Coin5RUBId INT;


------------------------------------------------------------
-- User
------------------------------------------------------------

IF NOT EXISTS
(
    SELECT 1
    FROM [dbo].[Users]
    WHERE [Username] = N'dev.user'
)
BEGIN
    INSERT INTO [dbo].[Users]
    (
        [Username],
        [PasswordHash],
        [Email],
        [IsActive]
    )
    VALUES
    (
        N'dev.user',
        N'DEVELOPMENT_ONLY_HASH',
        N'dev.user@coinsapp.local',
        1
    );
END;

SELECT @UserId = [UserId]
FROM [dbo].[Users]
WHERE [Username] = N'dev.user';


------------------------------------------------------------
-- Private Collection
------------------------------------------------------------

IF NOT EXISTS
(
    SELECT 1
    FROM [dbo].[Collections]
    WHERE [UserId] = @UserId
      AND [Name] = N'My Coin Collection'
)
BEGIN
    INSERT INTO [dbo].[Collections]
    (
        [UserId],
        [Name],
        [Description],
        [IsActive]
    )
    VALUES
    (
        @UserId,
        N'My Coin Collection',
        N'Development collection for testing.',
        1
    );
END;

SELECT @CollectionId = [CollectionId]
FROM [dbo].[Collections]
WHERE [UserId] = @UserId
  AND [Name] = N'My Coin Collection';


------------------------------------------------------------
-- Countries
------------------------------------------------------------

SELECT @GermanyId = [CountryId]
FROM [dbo].[Countries]
WHERE [Code] = N'DE';

SELECT @RussiaId = [CountryId]
FROM [dbo].[Countries]
WHERE [Code] = N'RU';


------------------------------------------------------------
-- Currencies
------------------------------------------------------------

SELECT @EURId = [CurrencyId]
FROM [dbo].[Currencies]
WHERE [Code] = N'EUR';

SELECT @DEMId = [CurrencyId]
FROM [dbo].[Currencies]
WHERE [Code] = N'DEM';

SELECT @RUBId = [CurrencyId]
FROM [dbo].[Currencies]
WHERE [Code] = N'RUB';


------------------------------------------------------------
-- Denominations
------------------------------------------------------------

SELECT @EUR2Id = [DenominationId]
FROM [dbo].[Denominations]
WHERE [CurrencyId] = @EURId
  AND [Value] = 2;

SELECT @DEM5Id = [DenominationId]
FROM [dbo].[Denominations]
WHERE [CurrencyId] = @DEMId
  AND [Value] = 5;

SELECT @RUB5Id = [DenominationId]
FROM [dbo].[Denominations]
WHERE [CurrencyId] = @RUBId
  AND [Value] = 5;


------------------------------------------------------------
-- Mints
------------------------------------------------------------

SELECT @BerlinMintId = [MintId]
FROM [dbo].[Mints]
WHERE [CountryId] = @GermanyId
  AND [Name] = N'Berlin';

SELECT @MoscowMintId = [MintId]
FROM [dbo].[Mints]
WHERE [CountryId] = @RussiaId
  AND [Name] = N'Moscow';


------------------------------------------------------------
-- Materials
------------------------------------------------------------

SELECT @SilverId = [MaterialId]
FROM [dbo].[Materials]
WHERE [Name] = N'Silver';

SELECT @CopperId = [MaterialId]
FROM [dbo].[Materials]
WHERE [Name] = N'Copper';


------------------------------------------------------------
-- Issuing Authorities
------------------------------------------------------------

SELECT @FederalGermanyId = [IssuingAuthorityId]
FROM [dbo].[IssuingAuthorities]
WHERE [CountryId] = @GermanyId
  AND [Name] = N'Federal Republic of Germany';

SELECT @SovietUnionId = [IssuingAuthorityId]
FROM [dbo].[IssuingAuthorities]
WHERE [CountryId] = @RussiaId
  AND [Name] = N'Soviet Union';


------------------------------------------------------------
-- Catalogs
------------------------------------------------------------

SELECT @KMId = [CatalogId]
FROM [dbo].[Catalogs]
WHERE [ShortName] = N'KM';

SELECT @JId = [CatalogId]
FROM [dbo].[Catalogs]
WHERE [ShortName] = N'J';


------------------------------------------------------------
-- Coin 1: Germany - 2 Euro
------------------------------------------------------------

IF NOT EXISTS
(
    SELECT 1
    FROM [dbo].[Coins]
    WHERE [CollectionId] = @CollectionId
      AND [Year] = 2024
      AND [CountryId] = @GermanyId
      AND [CurrencyId] = @EURId
      AND [DenominationId] = @EUR2Id
)
BEGIN
    INSERT INTO [dbo].[Coins]
    (
        [CollectionId],
        [CountryId],
        [IssuingAuthorityId],
        [CurrencyId],
        [DenominationId],
        [MintId],
        [MaterialId],
        [Year],
        [MintMark],
        [Weight],
        [Diameter],
        [Thickness],
        [Shape],
        [Description],
        [Condition],
        [CurrentPrice],
        [CurrentPriceCurrencyId],
        [CurrentPriceDate],
        [Notes]
    )
    VALUES
    (
        @CollectionId,
        @GermanyId,
        @FederalGermanyId,
        @EURId,
        @EUR2Id,
        @BerlinMintId,
        NULL,
        2024,
        N'A',
        8.50,
        25.75,
        2.20,
        N'Round',
        N'German 2 Euro circulation coin.',
        N'UNC',
        2.00,
        @EURId,
        SYSUTCDATETIME(),
        N'Development test coin.'
    );
END;


------------------------------------------------------------
-- Coin 2: Germany - 5 Deutsche Mark
------------------------------------------------------------

IF NOT EXISTS
(
    SELECT 1
    FROM [dbo].[Coins]
    WHERE [CollectionId] = @CollectionId
      AND [Year] = 1975
      AND [CountryId] = @GermanyId
      AND [CurrencyId] = @DEMId
      AND [DenominationId] = @DEM5Id
)
BEGIN
    INSERT INTO [dbo].[Coins]
    (
        [CollectionId],
        [CountryId],
        [IssuingAuthorityId],
        [CurrencyId],
        [DenominationId],
        [MintId],
        [MaterialId],
        [Year],
        [MintMark],
        [Fineness],
        [Weight],
        [Diameter],
        [Shape],
        [Description],
        [Condition],
        [CurrentPrice],
        [CurrentPriceCurrencyId],
        [CurrentPriceDate],
        [Notes]
    )
    VALUES
    (
        @CollectionId,
        @GermanyId,
        @FederalGermanyId,
        @DEMId,
        @DEM5Id,
        @BerlinMintId,
        @SilverId,
        1975,
        N'A',
        625.0000,
        11.20,
        29.00,
        N'Round',
        N'Historical German 5 Deutsche Mark coin.',
        N'VF',
        8.50,
        @EURId,
        SYSUTCDATETIME(),
        N'Historical currency test case.'
    );
END;


------------------------------------------------------------
-- Coin 3: Soviet Union - 5 Rubles
------------------------------------------------------------

IF NOT EXISTS
(
    SELECT 1
    FROM [dbo].[Coins]
    WHERE [CollectionId] = @CollectionId
      AND [Year] = 1987
      AND [CountryId] = @RussiaId
      AND [CurrencyId] = @RUBId
      AND [DenominationId] = @RUB5Id
)
BEGIN
    INSERT INTO [dbo].[Coins]
    (
        [CollectionId],
        [CountryId],
        [IssuingAuthorityId],
        [CurrencyId],
        [DenominationId],
        [MintId],
        [MaterialId],
        [Year],
        [MintMark],
        [Weight],
        [Diameter],
        [Shape],
        [Description],
        [Condition],
        [CurrentPrice],
        [CurrentPriceCurrencyId],
        [CurrentPriceDate],
        [Notes]
    )
    VALUES
    (
        @CollectionId,
        @RussiaId,
        @SovietUnionId,
        @RUBId,
        @RUB5Id,
        @MoscowMintId,
        @CopperId,
        1987,
        N'M',
        5.80,
        27.00,
        N'Round',
        N'Soviet 5 ruble commemorative coin.',
        N'XF',
        4.00,
        @EURId,
        SYSUTCDATETIME(),
        N'Historical issuing authority test case.'
    );
END;


------------------------------------------------------------
-- Resolve Coins
------------------------------------------------------------

SELECT @Coin2EuroId = [CoinId]
FROM [dbo].[Coins]
WHERE [CollectionId] = @CollectionId
  AND [Year] = 2024
  AND [CountryId] = @GermanyId
  AND [CurrencyId] = @EURId
  AND [DenominationId] = @EUR2Id;

SELECT @Coin5DEMId = [CoinId]
FROM [dbo].[Coins]
WHERE [CollectionId] = @CollectionId
  AND [Year] = 1975
  AND [CountryId] = @GermanyId
  AND [CurrencyId] = @DEMId
  AND [DenominationId] = @DEM5Id;

SELECT @Coin5RUBId = [CoinId]
FROM [dbo].[Coins]
WHERE [CollectionId] = @CollectionId
  AND [Year] = 1987
  AND [CountryId] = @RussiaId
  AND [CurrencyId] = @RUBId
  AND [DenominationId] = @RUB5Id;


------------------------------------------------------------
-- Catalog Entry: 2 Euro -> KM
------------------------------------------------------------

IF @KMId IS NOT NULL
AND @Coin2EuroId IS NOT NULL
AND NOT EXISTS
(
    SELECT 1
    FROM [dbo].[CatalogEntries]
    WHERE [CatalogId] = @KMId
      AND [CoinId] = @Coin2EuroId
)
BEGIN
    INSERT INTO [dbo].[CatalogEntries]
    (
        [CatalogId],
        [CoinId],
        [CatalogNumber],
        [Notes]
    )
    VALUES
    (
        @KMId,
        @Coin2EuroId,
        N'KM-2EUR-2024-DE',
        N'Development catalog entry.'
    );
END;


------------------------------------------------------------
-- Catalog Entry: 5 DEM -> KM
------------------------------------------------------------

IF @KMId IS NOT NULL
AND @Coin5DEMId IS NOT NULL
AND NOT EXISTS
(
    SELECT 1
    FROM [dbo].[CatalogEntries]
    WHERE [CatalogId] = @KMId
      AND [CoinId] = @Coin5DEMId
)
BEGIN
    INSERT INTO [dbo].[CatalogEntries]
    (
        [CatalogId],
        [CoinId],
        [CatalogNumber],
        [Notes]
    )
    VALUES
    (
        @KMId,
        @Coin5DEMId,
        N'KM-5DEM-1975-DE',
        N'Development catalog entry.'
    );
END;


------------------------------------------------------------
-- Catalog Entry: 5 DEM -> Jaeger
------------------------------------------------------------

IF @JId IS NOT NULL
AND @Coin5DEMId IS NOT NULL
AND NOT EXISTS
(
    SELECT 1
    FROM [dbo].[CatalogEntries]
    WHERE [CatalogId] = @JId
      AND [CoinId] = @Coin5DEMId
)
BEGIN
    INSERT INTO [dbo].[CatalogEntries]
    (
        [CatalogId],
        [CoinId],
        [CatalogNumber],
        [Notes]
    )
    VALUES
    (
        @JId,
        @Coin5DEMId,
        N'J-1975-5DM',
        N'Development catalog entry.'
    );
END;


------------------------------------------------------------
-- Catalog Entry: 5 RUB -> KM
------------------------------------------------------------

IF @KMId IS NOT NULL
AND @Coin5RUBId IS NOT NULL
AND NOT EXISTS
(
    SELECT 1
    FROM [dbo].[CatalogEntries]
    WHERE [CatalogId] = @KMId
      AND [CoinId] = @Coin5RUBId
)
BEGIN
    INSERT INTO [dbo].[CatalogEntries]
    (
        [CatalogId],
        [CoinId],
        [CatalogNumber],
        [Notes]
    )
    VALUES
    (
        @KMId,
        @Coin5RUBId,
        N'KM-5RUB-1987-USSR',
        N'Development catalog entry.'
    );
END;