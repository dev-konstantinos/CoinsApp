IF NOT EXISTS
(
    SELECT 1
    FROM [dbo].[Currencies]
    WHERE [Code] = N'EUR'
)
BEGIN
    INSERT INTO [dbo].[Currencies]
    (
        [Code],
        [Name],
        [Symbol],
        [IsActive]
    )
    VALUES
    (
        N'EUR',
        N'Euro',
        N'€',
        1
    );
END;


IF NOT EXISTS
(
    SELECT 1
    FROM [dbo].[Currencies]
    WHERE [Code] = N'DEM'
)
BEGIN
    INSERT INTO [dbo].[Currencies]
    (
        [Code],
        [Name],
        [Symbol],
        [IsActive]
    )
    VALUES
    (
        N'DEM',
        N'Deutsche Mark',
        N'DM',
        1
    );
END;


IF NOT EXISTS
(
    SELECT 1
    FROM [dbo].[Currencies]
    WHERE [Code] = N'RUB'
)
BEGIN
    INSERT INTO [dbo].[Currencies]
    (
        [Code],
        [Name],
        [Symbol],
        [IsActive]
    )
    VALUES
    (
        N'RUB',
        N'Russian Ruble',
        N'₽',
        1
    );
END;


IF NOT EXISTS
(
    SELECT 1
    FROM [dbo].[Currencies]
    WHERE [Code] = N'USD'
)
BEGIN
    INSERT INTO [dbo].[Currencies]
    (
        [Code],
        [Name],
        [Symbol],
        [IsActive]
    )
    VALUES
    (
        N'USD',
        N'US Dollar',
        N'$',
        1
    );
END;