DECLARE @EURId INT;
DECLARE @DEMId INT;
DECLARE @RUBId INT;
DECLARE @USDId INT;

SELECT @EURId = [CurrencyId]
FROM [dbo].[Currencies]
WHERE [Code] = N'EUR';

SELECT @DEMId = [CurrencyId]
FROM [dbo].[Currencies]
WHERE [Code] = N'DEM';

SELECT @RUBId = [CurrencyId]
FROM [dbo].[Currencies]
WHERE [Code] = N'RUB';

SELECT @USDId = [CurrencyId]
FROM [dbo].[Currencies]
WHERE [Code] = N'USD';


IF @EURId IS NOT NULL
AND NOT EXISTS
(
    SELECT 1
    FROM [dbo].[Denominations]
    WHERE [CurrencyId] = @EURId
      AND [Value] = 2
)
BEGIN
    INSERT INTO [dbo].[Denominations]
    (
        [CurrencyId],
        [Value],
        [DisplayName],
        [IsActive]
    )
    VALUES
    (
        @EURId,
        2,
        N'2 Euro',
        1
    );
END;


IF @DEMId IS NOT NULL
AND NOT EXISTS
(
    SELECT 1
    FROM [dbo].[Denominations]
    WHERE [CurrencyId] = @DEMId
      AND [Value] = 5
)
BEGIN
    INSERT INTO [dbo].[Denominations]
    (
        [CurrencyId],
        [Value],
        [DisplayName],
        [IsActive]
    )
    VALUES
    (
        @DEMId,
        5,
        N'5 Deutsche Mark',
        0
    );
END;


IF @RUBId IS NOT NULL
AND NOT EXISTS
(
    SELECT 1
    FROM [dbo].[Denominations]
    WHERE [CurrencyId] = @RUBId
      AND [Value] = 5
)
BEGIN
    INSERT INTO [dbo].[Denominations]
    (
        [CurrencyId],
        [Value],
        [DisplayName],
        [IsActive]
    )
    VALUES
    (
        @RUBId,
        5,
        N'5 Rubles',
        1
    );
END;


IF @USDId IS NOT NULL
AND NOT EXISTS
(
    SELECT 1
    FROM [dbo].[Denominations]
    WHERE [CurrencyId] = @USDId
      AND [Value] = 1
)
BEGIN
    INSERT INTO [dbo].[Denominations]
    (
        [CurrencyId],
        [Value],
        [DisplayName],
        [IsActive]
    )
    VALUES
    (
        @USDId,
        1,
        N'1 Dollar',
        1
    );
END;