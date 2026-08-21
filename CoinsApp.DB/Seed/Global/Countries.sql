IF NOT EXISTS
(
    SELECT 1
    FROM [dbo].[Countries]
    WHERE [Code] = N'DE'
)
BEGIN
    INSERT INTO [dbo].[Countries]
    (
        [Name],
        [Code],
        [IsActive]
    )
    VALUES
    (
        N'Germany',
        N'DE',
        1
    );
END;


IF NOT EXISTS
(
    SELECT 1
    FROM [dbo].[Countries]
    WHERE [Code] = N'FRG'
)
BEGIN
    INSERT INTO [dbo].[Countries]
    (
        [Name],
        [Code],
        [IsActive]
    )
    VALUES
    (
        N'Federal Republic of Germany',
        N'FRG',
        0
    );
END;


IF NOT EXISTS
(
    SELECT 1
    FROM [dbo].[Countries]
    WHERE [Code] = N'GDR'
)
BEGIN
    INSERT INTO [dbo].[Countries]
    (
        [Name],
        [Code],
        [IsActive]
    )
    VALUES
    (
        N'German Democratic Republic',
        N'GDR',
        0
    );
END;


IF NOT EXISTS
(
    SELECT 1
    FROM [dbo].[Countries]
    WHERE [Code] = N'USSR'
)
BEGIN
    INSERT INTO [dbo].[Countries]
    (
        [Name],
        [Code],
        [IsActive]
    )
    VALUES
    (
        N'Soviet Union',
        N'USSR',
        0
    );
END;


IF NOT EXISTS
(
    SELECT 1
    FROM [dbo].[Countries]
    WHERE [Code] = N'RE'
)
BEGIN
    INSERT INTO [dbo].[Countries]
    (
        [Name],
        [Code],
        [IsActive]
    )
    VALUES
    (
        N'Russian Empire',
        N'RE',
        0
    );
END;


IF NOT EXISTS
(
    SELECT 1
    FROM [dbo].[Countries]
    WHERE [Code] = N'RU'
)
BEGIN
    INSERT INTO [dbo].[Countries]
    (
        [Name],
        [Code],
        [IsActive]
    )
    VALUES
    (
        N'Russian Federation',
        N'RU',
        1
    );
END;


IF NOT EXISTS
(
    SELECT 1
    FROM [dbo].[Countries]
    WHERE [Code] = N'US'
)
BEGIN
    INSERT INTO [dbo].[Countries]
    (
        [Name],
        [Code],
        [IsActive]
    )
    VALUES
    (
        N'United States',
        N'US',
        1
    );
END;