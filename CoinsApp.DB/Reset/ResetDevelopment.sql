/*
    CoinsApp
    Development Reset

    Removes all development data belonging to dev.user.
    Transaction is controlled by DatabaseResetter.
*/

DECLARE @UserId INT;

SELECT
    @UserId = [UserId]
FROM [dbo].[Users]
WHERE [Username] = N'dev.user';

IF @UserId IS NOT NULL
BEGIN

    DELETE ci
    FROM [dbo].[CoinImages] AS ci
    INNER JOIN [dbo].[Coins] AS c
        ON c.[CoinId] = ci.[CoinId]
    INNER JOIN [dbo].[Collections] AS col
        ON col.[CollectionId] = c.[CollectionId]
    WHERE col.[UserId] = @UserId;

    DELETE ph
    FROM [dbo].[PriceHistory] AS ph
    INNER JOIN [dbo].[Coins] AS c
        ON c.[CoinId] = ph.[CoinId]
    INNER JOIN [dbo].[Collections] AS col
        ON col.[CollectionId] = c.[CollectionId]
    WHERE col.[UserId] = @UserId;

    DELETE p
    FROM [dbo].[Purchases] AS p
    INNER JOIN [dbo].[Coins] AS c
        ON c.[CoinId] = p.[CoinId]
    INNER JOIN [dbo].[Collections] AS col
        ON col.[CollectionId] = c.[CollectionId]
    WHERE col.[UserId] = @UserId;

    DELETE s
    FROM [dbo].[Sales] AS s
    INNER JOIN [dbo].[Coins] AS c
        ON c.[CoinId] = s.[CoinId]
    INNER JOIN [dbo].[Collections] AS col
        ON col.[CollectionId] = c.[CollectionId]
    WHERE col.[UserId] = @UserId;

    DELETE ce
    FROM [dbo].[CatalogEntries] AS ce
    INNER JOIN [dbo].[Coins] AS c
        ON c.[CoinId] = ce.[CoinId]
    INNER JOIN [dbo].[Collections] AS col
        ON col.[CollectionId] = c.[CollectionId]
    WHERE col.[UserId] = @UserId;

    DELETE c
    FROM [dbo].[Coins] AS c
    INNER JOIN [dbo].[Collections] AS col
        ON col.[CollectionId] = c.[CollectionId]
    WHERE col.[UserId] = @UserId;

    DELETE
    FROM [dbo].[Collections]
    WHERE [UserId] = @UserId;

    DELETE
    FROM [dbo].[Users]
    WHERE [UserId] = @UserId;

END;