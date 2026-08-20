IF NOT EXISTS
(
    SELECT 1
    FROM [dbo].[Catalogs]
    WHERE [Name] = N'Krause-Mishler Standard Catalog'
)
BEGIN
    INSERT INTO [dbo].[Catalogs]
    (
        [Name],
        [ShortName],
        [Publisher],
        [Description],
        [IsActive]
    )
    VALUES
    (
        N'Krause-Mishler Standard Catalog',
        N'KM',
        N'Krause Publications',
        N'World coin catalog.',
        1
    );
END;


IF NOT EXISTS
(
    SELECT 1
    FROM [dbo].[Catalogs]
    WHERE [Name] = N'Jaeger'
)
BEGIN
    INSERT INTO [dbo].[Catalogs]
    (
        [Name],
        [ShortName],
        [Publisher],
        [Description],
        [IsActive]
    )
    VALUES
    (
        N'Jaeger',
        N'J',
        NULL,
        N'German coin catalog.',
        1
    );
END;