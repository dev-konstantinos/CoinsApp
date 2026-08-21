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


------------------------------------------------------------
-- Countries
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
-- Germany -> Euro
------------------------------------------------------------

IF NOT EXISTS
(
    SELECT 1
    FROM [dbo].[CountryCurrencies]
    WHERE [CountryId] = @GermanyId
      AND [CurrencyId] = @EURId
)
BEGIN
    INSERT INTO [dbo].[CountryCurrencies]
    (
        [CountryId],
        [CurrencyId],
        [IsActive]
    )
    VALUES
    (
        @GermanyId,
        @EURId,
        1
    );
END;


------------------------------------------------------------
-- Germany -> Deutsche Mark
------------------------------------------------------------

IF NOT EXISTS
(
    SELECT 1
    FROM [dbo].[CountryCurrencies]
    WHERE [CountryId] = @GermanyId
      AND [CurrencyId] = @DEMId
)
BEGIN
    INSERT INTO [dbo].[CountryCurrencies]
    (
        [CountryId],
        [CurrencyId],
        [IsActive]
    )
    VALUES
    (
        @GermanyId,
        @DEMId,
        0
    );
END;


------------------------------------------------------------
-- Federal Republic of Germany -> Deutsche Mark
------------------------------------------------------------

IF NOT EXISTS
(
    SELECT 1
    FROM [dbo].[CountryCurrencies]
    WHERE [CountryId] = @FRGId
      AND [CurrencyId] = @DEMId
)
BEGIN
    INSERT INTO [dbo].[CountryCurrencies]
    (
        [CountryId],
        [CurrencyId],
        [IsActive]
    )
    VALUES
    (
        @FRGId,
        @DEMId,
        0
    );
END;


------------------------------------------------------------
-- German Democratic Republic -> Mark
------------------------------------------------------------

IF NOT EXISTS
(
    SELECT 1
    FROM [dbo].[CountryCurrencies]
    WHERE [CountryId] = @GDRId
      AND [CurrencyId] = @DDMId
)
BEGIN
    INSERT INTO [dbo].[CountryCurrencies]
    (
        [CountryId],
        [CurrencyId],
        [IsActive]
    )
    VALUES
    (
        @GDRId,
        @DDMId,
        0
    );
END;


------------------------------------------------------------
-- Soviet Union -> Soviet Ruble
------------------------------------------------------------

IF NOT EXISTS
(
    SELECT 1
    FROM [dbo].[CountryCurrencies]
    WHERE [CountryId] = @USSRId
      AND [CurrencyId] = @SURId
)
BEGIN
    INSERT INTO [dbo].[CountryCurrencies]
    (
        [CountryId],
        [CurrencyId],
        [IsActive]
    )
    VALUES
    (
        @USSRId,
        @SURId,
        0
    );
END;


------------------------------------------------------------
-- Russian Empire -> Russian Ruble
------------------------------------------------------------

IF NOT EXISTS
(
    SELECT 1
    FROM [dbo].[CountryCurrencies]
    WHERE [CountryId] = @RussianEmpireId
      AND [CurrencyId] = @RURId
)
BEGIN
    INSERT INTO [dbo].[CountryCurrencies]
    (
        [CountryId],
        [CurrencyId],
        [IsActive]
    )
    VALUES
    (
        @RussianEmpireId,
        @RURId,
        0
    );
END;


------------------------------------------------------------
-- Russian Federation -> Russian Ruble
------------------------------------------------------------

IF NOT EXISTS
(
    SELECT 1
    FROM [dbo].[CountryCurrencies]
    WHERE [CountryId] = @RussiaId
      AND [CurrencyId] = @RUBId
)
BEGIN
    INSERT INTO [dbo].[CountryCurrencies]
    (
        [CountryId],
        [CurrencyId],
        [IsActive]
    )
    VALUES
    (
        @RussiaId,
        @RUBId,
        1
    );
END;


------------------------------------------------------------
-- United States -> US Dollar
------------------------------------------------------------

IF NOT EXISTS
(
    SELECT 1
    FROM [dbo].[CountryCurrencies]
    WHERE [CountryId] = @USAId
      AND [CurrencyId] = @USDId
)
BEGIN
    INSERT INTO [dbo].[CountryCurrencies]
    (
        [CountryId],
        [CurrencyId],
        [IsActive]
    )
    VALUES
    (
        @USAId,
        @USDId,
        1
    );
END;