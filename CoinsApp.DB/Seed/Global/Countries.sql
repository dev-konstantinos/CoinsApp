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
        N'Russia',
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