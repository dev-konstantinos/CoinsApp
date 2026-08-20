DECLARE @GermanyId INT;
DECLARE @RussiaId INT;
DECLARE @UnitedStatesId INT;

SELECT @GermanyId = [CountryId]
FROM [dbo].[Countries]
WHERE [Code] = N'DE';

SELECT @RussiaId = [CountryId]
FROM [dbo].[Countries]
WHERE [Code] = N'RU';

SELECT @UnitedStatesId = [CountryId]
FROM [dbo].[Countries]
WHERE [Code] = N'US';


IF @GermanyId IS NOT NULL
AND NOT EXISTS
(
    SELECT 1
    FROM [dbo].[IssuingAuthorities]
    WHERE [CountryId] = @GermanyId
      AND [Name] = N'German Empire'
)
BEGIN
    INSERT INTO [dbo].[IssuingAuthorities]
    (
        [CountryId],
        [Name],
        [Description],
        [IsActive]
    )
    VALUES
    (
        @GermanyId,
        N'German Empire',
        N'German Empire (1871-1918)',
        1
    );
END;


IF @GermanyId IS NOT NULL
AND NOT EXISTS
(
    SELECT 1
    FROM [dbo].[IssuingAuthorities]
    WHERE [CountryId] = @GermanyId
      AND [Name] = N'Federal Republic of Germany'
)
BEGIN
    INSERT INTO [dbo].[IssuingAuthorities]
    (
        [CountryId],
        [Name],
        [Description],
        [IsActive]
    )
    VALUES
    (
        @GermanyId,
        N'Federal Republic of Germany',
        N'Federal Republic of Germany',
        1
    );
END;

IF @RussiaId IS NOT NULL
AND NOT EXISTS
(
    SELECT 1
    FROM [dbo].[IssuingAuthorities]
    WHERE [CountryId] = @RussiaId
      AND [Name] = N'Soviet Union'
)
BEGIN
    INSERT INTO [dbo].[IssuingAuthorities]
    (
        [CountryId],
        [Name],
        [Description],
        [IsActive]
    )
    VALUES
    (
        @RussiaId,
        N'Soviet Union',
        N'Union of Soviet Socialist Republics (USSR)',
        1
    );
END;

IF @RussiaId IS NOT NULL
AND NOT EXISTS
(
    SELECT 1
    FROM [dbo].[IssuingAuthorities]
    WHERE [CountryId] = @RussiaId
      AND [Name] = N'Russian Federation'
)
BEGIN
    INSERT INTO [dbo].[IssuingAuthorities]
    (
        [CountryId],
        [Name],
        [Description],
        [IsActive]
    )
    VALUES
    (
        @RussiaId,
        N'Russian Federation',
        N'Russian Federation',
        1
    );
END;


IF @UnitedStatesId IS NOT NULL
AND NOT EXISTS
(
    SELECT 1
    FROM [dbo].[IssuingAuthorities]
    WHERE [CountryId] = @UnitedStatesId
      AND [Name] = N'United States of America'
)
BEGIN
    INSERT INTO [dbo].[IssuingAuthorities]
    (
        [CountryId],
        [Name],
        [Description],
        [IsActive]
    )
    VALUES
    (
        @UnitedStatesId,
        N'United States of America',
        N'United States of America',
        1
    );
END;