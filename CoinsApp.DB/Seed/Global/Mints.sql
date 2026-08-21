DECLARE @GermanyId INT;
DECLARE @FRGId INT;
DECLARE @GDRId INT;
DECLARE @USSRId INT;
DECLARE @RussianEmpireId INT;
DECLARE @RussiaId INT;
DECLARE @UnitedStatesId INT;


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

SELECT @UnitedStatesId = [CountryId]
FROM [dbo].[Countries]
WHERE [Code] = N'US';


------------------------------------------------------------
-- Germany - Berlin Mint
------------------------------------------------------------

IF @GermanyId IS NOT NULL
AND NOT EXISTS
(
    SELECT 1
    FROM [dbo].[Mints]
    WHERE [CountryId] = @GermanyId
      AND [Name] = N'Berlin'
)
BEGIN
    INSERT INTO [dbo].[Mints]
    (
        [CountryId],
        [Name],
        [Code],
        [City],
        [IsActive]
    )
    VALUES
    (
        @GermanyId,
        N'Berlin',
        N'A',
        N'Berlin',
        1
    );
END;


------------------------------------------------------------
-- Federal Republic of Germany - Berlin Mint
------------------------------------------------------------

IF @FRGId IS NOT NULL
AND NOT EXISTS
(
    SELECT 1
    FROM [dbo].[Mints]
    WHERE [CountryId] = @FRGId
      AND [Name] = N'Berlin'
)
BEGIN
    INSERT INTO [dbo].[Mints]
    (
        [CountryId],
        [Name],
        [Code],
        [City],
        [IsActive]
    )
    VALUES
    (
        @FRGId,
        N'Berlin',
        N'A',
        N'Berlin',
        0
    );
END;


------------------------------------------------------------
-- German Democratic Republic - Berlin Mint
------------------------------------------------------------

IF @GDRId IS NOT NULL
AND NOT EXISTS
(
    SELECT 1
    FROM [dbo].[Mints]
    WHERE [CountryId] = @GDRId
      AND [Name] = N'Berlin'
)
BEGIN
    INSERT INTO [dbo].[Mints]
    (
        [CountryId],
        [Name],
        [Code],
        [City],
        [IsActive]
    )
    VALUES
    (
        @GDRId,
        N'Berlin',
        N'A',
        N'Berlin',
        0
    );
END;


------------------------------------------------------------
-- Soviet Union - Moscow Mint
------------------------------------------------------------

IF @USSRId IS NOT NULL
AND NOT EXISTS
(
    SELECT 1
    FROM [dbo].[Mints]
    WHERE [CountryId] = @USSRId
      AND [Name] = N'Moscow'
)
BEGIN
    INSERT INTO [dbo].[Mints]
    (
        [CountryId],
        [Name],
        [Code],
        [City],
        [IsActive]
    )
    VALUES
    (
        @USSRId,
        N'Moscow',
        N'M',
        N'Moscow',
        0
    );
END;


------------------------------------------------------------
-- Russian Empire - St. Petersburg Mint
------------------------------------------------------------

IF @RussianEmpireId IS NOT NULL
AND NOT EXISTS
(
    SELECT 1
    FROM [dbo].[Mints]
    WHERE [CountryId] = @RussianEmpireId
      AND [Name] = N'St. Petersburg'
)
BEGIN
    INSERT INTO [dbo].[Mints]
    (
        [CountryId],
        [Name],
        [Code],
        [City],
        [IsActive]
    )
    VALUES
    (
        @RussianEmpireId,
        N'St. Petersburg',
        N'SP',
        N'St. Petersburg',
        0
    );
END;


------------------------------------------------------------
-- Russian Federation - Moscow Mint
------------------------------------------------------------

IF @RussiaId IS NOT NULL
AND NOT EXISTS
(
    SELECT 1
    FROM [dbo].[Mints]
    WHERE [CountryId] = @RussiaId
      AND [Name] = N'Moscow'
)
BEGIN
    INSERT INTO [dbo].[Mints]
    (
        [CountryId],
        [Name],
        [Code],
        [City],
        [IsActive]
    )
    VALUES
    (
        @RussiaId,
        N'Moscow',
        N'M',
        N'Moscow',
        1
    );
END;


------------------------------------------------------------
-- United States - Philadelphia Mint
------------------------------------------------------------

IF @UnitedStatesId IS NOT NULL
AND NOT EXISTS
(
    SELECT 1
    FROM [dbo].[Mints]
    WHERE [CountryId] = @UnitedStatesId
      AND [Name] = N'Philadelphia'
)
BEGIN
    INSERT INTO [dbo].[Mints]
    (
        [CountryId],
        [Name],
        [Code],
        [City],
        [IsActive]
    )
    VALUES
    (
        @UnitedStatesId,
        N'Philadelphia',
        N'P',
        N'Philadelphia',
        1
    );
END;