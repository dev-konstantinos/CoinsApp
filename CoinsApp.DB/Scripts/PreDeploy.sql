/*
    CoinsApp Pre-Deployment Script

    Guard the Sales.BuyerId nullability migration.
    Existing NULL BuyerId values cannot be repaired automatically
    without inventing a buyer.
*/

IF OBJECT_ID(N'[dbo].[Sales]', N'U') IS NOT NULL
BEGIN
    IF EXISTS
    (
        SELECT 1
        FROM [dbo].[Sales]
        WHERE [BuyerId] IS NULL
    )
    BEGIN
        RAISERROR(
            'Sales migration blocked: existing Sales contain NULL BuyerId values. Repair these records before deploying the BuyerId rule.',
            16,
            1
        );

        RETURN;
    END;
END;