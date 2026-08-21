DECLARE @EURId INT;
DECLARE @DEMId INT;
DECLARE @DDMId INT;
DECLARE @SURId INT;
DECLARE @RURId INT;
DECLARE @RUBId INT;
DECLARE @USDId INT;


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
-- Euro
------------------------------------------------------------

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


------------------------------------------------------------
-- Deutsche Mark
------------------------------------------------------------

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


------------------------------------------------------------
-- Mark der DDR
------------------------------------------------------------

IF @DDMId IS NOT NULL
AND NOT EXISTS
(
    SELECT 1
    FROM [dbo].[Denominations]
    WHERE [CurrencyId] = @DDMId
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
        @DDMId,
        5,
        N'5 Mark',
        0
    );
END;


------------------------------------------------------------
-- Soviet Ruble
------------------------------------------------------------

IF @SURId IS NOT NULL
AND NOT EXISTS
(
    SELECT 1
    FROM [dbo].[Denominations]
    WHERE [CurrencyId] = @SURId
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
        @SURId,
        5,
        N'5 Soviet Rubles',
        0
    );
END;


------------------------------------------------------------
-- Russian Empire Ruble
------------------------------------------------------------

IF @RURId IS NOT NULL
AND NOT EXISTS
(
    SELECT 1
    FROM [dbo].[Denominations]
    WHERE [CurrencyId] = @RURId
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
        @RURId,
        5,
        N'5 Russian Rubles',
        0
    );
END;


------------------------------------------------------------
-- Russian Ruble
------------------------------------------------------------

IF @RUBId IS NOT NULL
AND NOT EXISTS
(
    SELECT 1
    FROM [dbo].[Denominations]
    WHERE [CurrencyId] = @RUBId
      AND [Value] = 10
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
        10,
        N'10 Rubles',
        1
    );
END;


------------------------------------------------------------
-- US Dollar
------------------------------------------------------------

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