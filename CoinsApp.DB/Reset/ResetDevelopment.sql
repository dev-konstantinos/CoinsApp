/*
    CoinsApp
    Development Reset

    Removes all development data belonging to dev.user.
    This script only removes user-owned development data.
*/

SET XACT_ABORT ON;

BEGIN TRY

    BEGIN TRANSACTION;

    DECLARE @UserId INT;

    SELECT
        @UserId = [UserId]
    FROM [dbo].[Users]
    WHERE [Username] = N'dev.user';


    IF @UserId IS NOT NULL
    BEGIN

        /*
            1. CoinImages
        */
        DELETE ci
        FROM [dbo].[CoinImages] AS ci
        INNER JOIN [dbo].[Coins] AS c
            ON c.[CoinId] = ci.[CoinId]
        INNER JOIN [dbo].[Collections] AS col
            ON col.[CollectionId] = c.[CollectionId]
        WHERE col.[UserId] = @UserId;


        /*
            2. PriceHistory
        */
        DELETE ph
        FROM [dbo].[PriceHistory] AS ph
        INNER JOIN [dbo].[Coins] AS c
            ON c.[CoinId] = ph.[CoinId]
        INNER JOIN [dbo].[Collections] AS col
            ON col.[CollectionId] = c.[CollectionId]
        WHERE col.[UserId] = @UserId;


        /*
            3. Purchases
        */
        DELETE p
        FROM [dbo].[Purchases] AS p
        INNER JOIN [dbo].[Coins] AS c
            ON c.[CoinId] = p.[CoinId]
        INNER JOIN [dbo].[Collections] AS col
            ON col.[CollectionId] = c.[CollectionId]
        WHERE col.[UserId] = @UserId;


        /*
            4. Sales
        */
        DELETE s
        FROM [dbo].[Sales] AS s
        INNER JOIN [dbo].[Coins] AS c
            ON c.[CoinId] = s.[CoinId]
        INNER JOIN [dbo].[Collections] AS col
            ON col.[CollectionId] = c.[CollectionId]
        WHERE col.[UserId] = @UserId;


        /*
            5. CatalogEntries
        */
        DELETE ce
        FROM [dbo].[CatalogEntries] AS ce
        INNER JOIN [dbo].[Coins] AS c
            ON c.[CoinId] = ce.[CoinId]
        INNER JOIN [dbo].[Collections] AS col
            ON col.[CollectionId] = c.[CollectionId]
        WHERE col.[UserId] = @UserId;


        /*
            6. Coins
        */
        DELETE c
        FROM [dbo].[Coins] AS c
        INNER JOIN [dbo].[Collections] AS col
            ON col.[CollectionId] = c.[CollectionId]
        WHERE col.[UserId] = @UserId;


        /*
            7. Collections
        */
        DELETE
        FROM [dbo].[Collections]
        WHERE [UserId] = @UserId;


        /*
            8. Development user
        */
        DELETE
        FROM [dbo].[Users]
        WHERE [UserId] = @UserId;

    END;


    COMMIT TRANSACTION;

END TRY
BEGIN CATCH

    IF @@TRANCOUNT > 0
    BEGIN
        ROLLBACK TRANSACTION;
    END;

    THROW;

END CATCH;