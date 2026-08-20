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
    FROM [dbo].[Mints]
    WHERE [Name] = N'Berlin'
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


IF @RussiaId IS NOT NULL
AND NOT EXISTS
(
    SELECT 1
    FROM [dbo].[Mints]
    WHERE [Name] = N'Moscow'
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


IF @UnitedStatesId IS NOT NULL
AND NOT EXISTS
(
    SELECT 1
    FROM [dbo].[Mints]
    WHERE [Name] = N'Philadelphia'
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