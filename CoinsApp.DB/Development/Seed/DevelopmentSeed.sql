/*
    CoinsApp
    Development Seed

    Development/test data only.
    This script is NOT executed by PostDeploy.
*/

DECLARE @UserId INT;
DECLARE @CollectionId INT;

DECLARE @GermanyId INT;
DECLARE @FRGId INT;
DECLARE @GDRId INT;
DECLARE @USSRId INT;
DECLARE @RussianEmpireId INT;
DECLARE @RussiaId INT;
DECLARE @USAId INT;

DECLARE @EURId INT;
DECLARE @DEMId INT;
DECLARE @DDMId INT;
DECLARE @SURId INT;
DECLARE @RURId INT;
DECLARE @RUBId INT;
DECLARE @USDId INT;

DECLARE @EUR2Id INT;
DECLARE @DEM5Id INT;
DECLARE @DDM5Id INT;
DECLARE @SUR5Id INT;
DECLARE @RUR5Id INT;
DECLARE @RUB10Id INT;
DECLARE @USD1Id INT;

DECLARE @GermanyBerlinMintId INT;
DECLARE @FRGBerlinMintId INT;
DECLARE @GDRBerlinMintId INT;
DECLARE @USSRMoscowMintId INT;
DECLARE @RussianEmpireStPetersburgMintId INT;
DECLARE @RussiaMoscowMintId INT;
DECLARE @USAPhiladelphiaMintId INT;

DECLARE @SilverId INT;
DECLARE @CopperId INT;


------------------------------------------------------------
-- User
------------------------------------------------------------

SELECT @UserId = [UserId]
FROM [dbo].[Users]
WHERE [Username] = N'dev.user';


IF @UserId IS NULL
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

    SET @UserId = SCOPE_IDENTITY();
END;


------------------------------------------------------------
-- Collection
------------------------------------------------------------

SELECT @CollectionId = [CollectionId]
FROM [dbo].[Collections]
WHERE [UserId] = @UserId
  AND [Name] = N'My Coin Collection';


IF @CollectionId IS NULL
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

    SET @CollectionId = SCOPE_IDENTITY();
END;


------------------------------------------------------------
-- Countries / Issuers
------------------------------------------------------------

SELECT @GermanyId = [CountryId]
FROM [dbo].[Countries]
WHERE [Code] = N'DE';

SELECT @FRGId = [CountryId]
FROM [dbo].[Countries]
WHERE [Code] = N'FRG';

SELECT @GDRId = [CountryId]
FROM [dbo].[Countries]
WHERE [Code] = N'GDR';

SELECT @USSRId = [CountryId]
FROM [dbo].[Countries]
WHERE [Code] = N'USSR';

SELECT @RussianEmpireId = [CountryId]
FROM [dbo].[Countries]
WHERE [Code] = N'RE';

SELECT @RussiaId = [CountryId]
FROM [dbo].[Countries]
WHERE [Code] = N'RU';

SELECT @USAId = [CountryId]
FROM [dbo].[Countries]
WHERE [Code] = N'US';


------------------------------------------------------------
-- Currencies
------------------------------------------------------------

SELECT @EURId = [CurrencyId]
FROM [dbo].[Currencies]
WHERE [Code] = N'EUR';

SELECT @DEMId = [CurrencyId]
FROM [dbo].[Currencies]
WHERE [Code] = N'DEM';

SELECT @DDMId = [CurrencyId]
FROM [dbo].[Currencies]
WHERE [Code] = N'DDM';

SELECT @SURId = [CurrencyId]
FROM [dbo].[Currencies]
WHERE [Code] = N'SUR';

SELECT @RURId = [CurrencyId]
FROM [dbo].[Currencies]
WHERE [Code] = N'RUR';

SELECT @RUBId = [CurrencyId]
FROM [dbo].[Currencies]
WHERE [Code] = N'RUB';

SELECT @USDId = [CurrencyId]
FROM [dbo].[Currencies]
WHERE [Code] = N'USD';


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

SELECT @DDM5Id = [DenominationId]
FROM [dbo].[Denominations]
WHERE [CurrencyId] = @DDMId
  AND [Value] = 5;

SELECT @SUR5Id = [DenominationId]
FROM [dbo].[Denominations]
WHERE [CurrencyId] = @SURId
  AND [Value] = 5;

SELECT @RUR5Id = [DenominationId]
FROM [dbo].[Denominations]
WHERE [CurrencyId] = @RURId
  AND [Value] = 5;

SELECT @RUB10Id = [DenominationId]
FROM [dbo].[Denominations]
WHERE [CurrencyId] = @RUBId
  AND [Value] = 10;

SELECT @USD1Id = [DenominationId]
FROM [dbo].[Denominations]
WHERE [CurrencyId] = @USDId
  AND [Value] = 1;


------------------------------------------------------------
-- Mints
------------------------------------------------------------

SELECT @GermanyBerlinMintId = [MintId]
FROM [dbo].[Mints]
WHERE [CountryId] = @GermanyId
  AND [Name] = N'Berlin';

SELECT @FRGBerlinMintId = [MintId]
FROM [dbo].[Mints]
WHERE [CountryId] = @FRGId
  AND [Name] = N'Berlin';

SELECT @GDRBerlinMintId = [MintId]
FROM [dbo].[Mints]
WHERE [CountryId] = @GDRId
  AND [Name] = N'Berlin';

SELECT @USSRMoscowMintId = [MintId]
FROM [dbo].[Mints]
WHERE [CountryId] = @USSRId
  AND [Name] = N'Moscow';

SELECT @RussianEmpireStPetersburgMintId = [MintId]
FROM [dbo].[Mints]
WHERE [CountryId] = @RussianEmpireId
  AND [Name] = N'St. Petersburg';

SELECT @RussiaMoscowMintId = [MintId]
FROM [dbo].[Mints]
WHERE [CountryId] = @RussiaId
  AND [Name] = N'Moscow';

SELECT @USAPhiladelphiaMintId = [MintId]
FROM [dbo].[Mints]
WHERE [CountryId] = @USAId
  AND [Name] = N'Philadelphia';


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
-- Germany
-- 2 Euro, 2024
------------------------------------------------------------

IF NOT EXISTS
(
    SELECT 1
    FROM [dbo].[Coins]
    WHERE [CollectionId] = @CollectionId
      AND [CountryId] = @GermanyId
      AND [CurrencyId] = @EURId
      AND [DenominationId] = @EUR2Id
      AND [Year] = 2024
)
BEGIN
    INSERT INTO [dbo].[Coins]
    (
        [CollectionId],
        [CountryId],
        [CurrencyId],
        [DenominationId],
        [MintId],
        [MaterialId],
        [Year],
        [MintMark],
        [Fineness],
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
        @EURId,
        @EUR2Id,
        @GermanyBerlinMintId,
        NULL,
        2024,
        N'A',
        NULL,
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
-- Federal Republic of Germany
-- 5 Deutsche Mark, 1975
------------------------------------------------------------

IF NOT EXISTS
(
    SELECT 1
    FROM [dbo].[Coins]
    WHERE [CollectionId] = @CollectionId
      AND [CountryId] = @FRGId
      AND [CurrencyId] = @DEMId
      AND [DenominationId] = @DEM5Id
      AND [Year] = 1975
)
BEGIN
    INSERT INTO [dbo].[Coins]
    (
        [CollectionId],
        [CountryId],
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
        @FRGId,
        @DEMId,
        @DEM5Id,
        @FRGBerlinMintId,
        @SilverId,
        1975,
        N'A',
        625.0000,
        11.20,
        29.00,
        N'Round',
        N'West German 5 Deutsche Mark silver coin.',
        N'VF',
        8.50,
        @EURId,
        SYSUTCDATETIME(),
        N'Historical issuer development test coin.'
    );
END;


------------------------------------------------------------
-- German Democratic Republic
-- 5 Mark, 1975
------------------------------------------------------------

IF NOT EXISTS
(
    SELECT 1
    FROM [dbo].[Coins]
    WHERE [CollectionId] = @CollectionId
      AND [CountryId] = @GDRId
      AND [CurrencyId] = @DDMId
      AND [DenominationId] = @DDM5Id
      AND [Year] = 1975
)
BEGIN
    INSERT INTO [dbo].[Coins]
    (
        [CollectionId],
        [CountryId],
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
        @GDRId,
        @DDMId,
        @DDM5Id,
        @GDRBerlinMintId,
        @CopperId,
        1975,
        NULL,
        NULL,
        9.60,
        29.00,
        N'Round',
        N'East German 5 Mark coin.',
        N'VF',
        5.00,
        @EURId,
        SYSUTCDATETIME(),
        N'Historical issuer development test coin.'
    );
END;


------------------------------------------------------------
-- Soviet Union
-- 5 Soviet Rubles, 1987
------------------------------------------------------------

IF NOT EXISTS
(
    SELECT 1
    FROM [dbo].[Coins]
    WHERE [CollectionId] = @CollectionId
      AND [CountryId] = @USSRId
      AND [CurrencyId] = @SURId
      AND [DenominationId] = @SUR5Id
      AND [Year] = 1987
)
BEGIN
    INSERT INTO [dbo].[Coins]
    (
        [CollectionId],
        [CountryId],
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
        @USSRId,
        @SURId,
        @SUR5Id,
        @USSRMoscowMintId,
        @CopperId,
        1987,
        N'M',
        NULL,
        5.80,
        27.00,
        N'Round',
        N'Soviet 5 Rubles commemorative coin.',
        N'XF',
        4.00,
        @EURId,
        SYSUTCDATETIME(),
        N'Historical issuer development test coin.'
    );
END;


------------------------------------------------------------
-- Russian Empire
-- 5 Rubles, 1899
------------------------------------------------------------

IF NOT EXISTS
(
    SELECT 1
    FROM [dbo].[Coins]
    WHERE [CollectionId] = @CollectionId
      AND [CountryId] = @RussianEmpireId
      AND [CurrencyId] = @RURId
      AND [DenominationId] = @RUR5Id
      AND [Year] = 1899
)
BEGIN
    INSERT INTO [dbo].[Coins]
    (
        [CollectionId],
        [CountryId],
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
        @RussianEmpireId,
        @RURId,
        @RUR5Id,
        @RussianEmpireStPetersburgMintId,
        @SilverId,
        1899,
        NULL,
        900.0000,
        4.30,
        18.50,
        N'Round',
        N'Russian Empire 5 Rubles silver coin.',
        N'VF',
        500.00,
        @EURId,
        SYSUTCDATETIME(),
        N'Historical issuer development test coin.'
    );
END;


------------------------------------------------------------
-- Russian Federation
-- 10 Rubles, 2024
------------------------------------------------------------

IF NOT EXISTS
(
    SELECT 1
    FROM [dbo].[Coins]
    WHERE [CollectionId] = @CollectionId
      AND [CountryId] = @RussiaId
      AND [CurrencyId] = @RUBId
      AND [DenominationId] = @RUB10Id
      AND [Year] = 2024
)
BEGIN
    INSERT INTO [dbo].[Coins]
    (
        [CollectionId],
        [CountryId],
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
        @RussiaId,
        @RUBId,
        @RUB10Id,
        @RussiaMoscowMintId,
        @CopperId,
        2024,
        N'M',
        NULL,
        5.63,
        22.00,
        N'Round',
        N'Russian Federation 10 Rubles coin.',
        N'UNC',
        0.15,
        @EURId,
        SYSUTCDATETIME(),
        N'Current issuer development test coin.'
    );
END;


------------------------------------------------------------
-- United States
-- 1 Dollar, 2024
------------------------------------------------------------

IF NOT EXISTS
(
    SELECT 1
    FROM [dbo].[Coins]
    WHERE [CollectionId] = @CollectionId
      AND [CountryId] = @USAId
      AND [CurrencyId] = @USDId
      AND [DenominationId] = @USD1Id
      AND [Year] = 2024
)
BEGIN
    INSERT INTO [dbo].[Coins]
    (
        [CollectionId],
        [CountryId],
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
        @USAId,
        @USDId,
        @USD1Id,
        @USAPhiladelphiaMintId,
        @CopperId,
        2024,
        N'P',
        NULL,
        8.10,
        26.50,
        N'Round',
        N'United States 1 Dollar coin.',
        N'UNC',
        1.00,
        @EURId,
        SYSUTCDATETIME(),
        N'Development test coin.'
    );
END;