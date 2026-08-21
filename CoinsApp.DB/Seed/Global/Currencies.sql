IF NOT EXISTS
(
    SELECT 1
    FROM [dbo].[Currencies]
    WHERE [Code] = N'EUR'
)
BEGIN
    INSERT INTO [dbo].[Currencies]
    (
        [Name],
        [Code],
        [Symbol],
        [IsActive]
    )
    VALUES
    (
        N'Euro',
        N'EUR',
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
        [Name],
        [Code],
        [Symbol],
        [IsActive]
    )
    VALUES
    (
        N'Deutsche Mark',
        N'DEM',
        N'DM',
        0
    );
END;


IF NOT EXISTS
(
    SELECT 1
    FROM [dbo].[Currencies]
    WHERE [Code] = N'DDM'
)
BEGIN
    INSERT INTO [dbo].[Currencies]
    (
        [Name],
        [Code],
        [Symbol],
        [IsActive]
    )
    VALUES
    (
        N'Mark der DDR',
        N'DDM',
        N'M',
        0
    );
END;


IF NOT EXISTS
(
    SELECT 1
    FROM [dbo].[Currencies]
    WHERE [Code] = N'SUR'
)
BEGIN
    INSERT INTO [dbo].[Currencies]
    (
        [Name],
        [Code],
        [Symbol],
        [IsActive]
    )
    VALUES
    (
        N'Soviet Ruble',
        N'SUR',
        N'₽',
        0
    );
END;


IF NOT EXISTS
(
    SELECT 1
    FROM [dbo].[Currencies]
    WHERE [Code] = N'RUR'
)
BEGIN
    INSERT INTO [dbo].[Currencies]
    (
        [Name],
        [Code],
        [Symbol],
        [IsActive]
    )
    VALUES
    (
        N'Russian Ruble',
        N'RUR',
        N'₽',
        0
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
        [Name],
        [Code],
        [Symbol],
        [IsActive]
    )
    VALUES
    (
        N'Russian Ruble',
        N'RUB',
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
        [Name],
        [Code],
        [Symbol],
        [IsActive]
    )
    VALUES
    (
        N'United States Dollar',
        N'USD',
        N'$',
        1
    );
END;