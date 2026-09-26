IF NOT EXISTS
(
    SELECT 1
    FROM [dbo].[Materials]
    WHERE [Name] = N'Silver'
)
BEGIN
    INSERT INTO [dbo].[Materials]
    (
        [Name],
        [Symbol],
        [IsPreciousMetal],
        [IsActive]
    )
    VALUES
    (
        N'Silver',
        N'Ag',
        1,
        1
    );
END;


IF NOT EXISTS
(
    SELECT 1
    FROM [dbo].[Materials]
    WHERE [Name] = N'Gold'
)
BEGIN
    INSERT INTO [dbo].[Materials]
    (
        [Name],
        [Symbol],
        [IsPreciousMetal],
        [IsActive]
    )
    VALUES
    (
        N'Gold',
        N'Au',
        1,
        1
    );
END;


IF NOT EXISTS
(
    SELECT 1
    FROM [dbo].[Materials]
    WHERE [Name] = N'Copper'
)
BEGIN
    INSERT INTO [dbo].[Materials]
    (
        [Name],
        [Symbol],
        [IsPreciousMetal],
        [IsActive]
    )
    VALUES
    (
        N'Copper',
        N'Cu',
        0,
        1
    );
END;