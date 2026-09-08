/*
    CoinsApp
    Development Reset

    Removes all development data belonging to dev.user.
    Development contacts used by the seed are removed as well.

    Transaction is controlled by DatabaseResetter.
*/

DECLARE @UserId INT;
DECLARE @OwnerId INT;
DECLARE @BuyerId INT;


------------------------------------------------------------
-- Resolve development user
------------------------------------------------------------

SELECT
    @UserId = [UserId]
FROM [dbo].[Users]
WHERE [Username] = N'dev.user';


------------------------------------------------------------
-- Resolve development contacts
------------------------------------------------------------

SELECT
    @OwnerId = [ContactId]
FROM [dbo].[Contacts]
WHERE [Email] = N'dev.owner@coinsapp.local';


SELECT
    @BuyerId = [ContactId]
FROM [dbo].[Contacts]
WHERE [Email] = N'dev.buyer@coinsapp.local';


------------------------------------------------------------
-- Delete development data
------------------------------------------------------------

IF @UserId IS NOT NULL
BEGIN

    --------------------------------------------------------
    -- Coin Images
    --------------------------------------------------------

    DELETE ci
    FROM [dbo].[CoinImages] AS ci
    INNER JOIN [dbo].[Coins] AS c
        ON c.[CoinId] = ci.[CoinId]
    INNER JOIN [dbo].[Collections] AS col
        ON col.[CollectionId] = c.[CollectionId]
    WHERE col.[UserId] = @UserId;


    --------------------------------------------------------
    -- Price History
    --------------------------------------------------------

    DELETE ph
    FROM [dbo].[PriceHistory] AS ph
    INNER JOIN [dbo].[Coins] AS c
        ON c.[CoinId] = ph.[CoinId]
    INNER JOIN [dbo].[Collections] AS col
        ON col.[CollectionId] = c.[CollectionId]
    WHERE col.[UserId] = @UserId;


    --------------------------------------------------------
    -- Purchases
    --------------------------------------------------------

    DELETE p
    FROM [dbo].[Purchases] AS p
    INNER JOIN [dbo].[Coins] AS c
        ON c.[CoinId] = p.[CoinId]
    INNER JOIN [dbo].[Collections] AS col
        ON col.[CollectionId] = c.[CollectionId]
    WHERE col.[UserId] = @UserId;


    --------------------------------------------------------
    -- Sales
    --------------------------------------------------------

    DELETE s
    FROM [dbo].[Sales] AS s
    INNER JOIN [dbo].[Coins] AS c
        ON c.[CoinId] = s.[CoinId]
    INNER JOIN [dbo].[Collections] AS col
        ON col.[CollectionId] = c.[CollectionId]
    WHERE col.[UserId] = @UserId;


    --------------------------------------------------------
    -- Catalog Entries
    --------------------------------------------------------

    DELETE ce
    FROM [dbo].[CatalogEntries] AS ce
    INNER JOIN [dbo].[Coins] AS c
        ON c.[CoinId] = ce.[CoinId]
    INNER JOIN [dbo].[Collections] AS col
        ON col.[CollectionId] = c.[CollectionId]
    WHERE col.[UserId] = @UserId;


    --------------------------------------------------------
    -- Coins
    --------------------------------------------------------

    DELETE c
    FROM [dbo].[Coins] AS c
    INNER JOIN [dbo].[Collections] AS col
        ON col.[CollectionId] = c.[CollectionId]
    WHERE col.[UserId] = @UserId;


    --------------------------------------------------------
    -- Collections
    --------------------------------------------------------

    DELETE
    FROM [dbo].[Collections]
    WHERE [UserId] = @UserId;


    --------------------------------------------------------
    -- Development User
    --------------------------------------------------------

    DELETE
    FROM [dbo].[Users]
    WHERE [UserId] = @UserId;
END;


------------------------------------------------------------
-- Development Contacts
--
-- Coins have already been deleted above, so OwnerId/BayerId
-- foreign-key references no longer exist.
------------------------------------------------------------

DELETE
FROM [dbo].[Contacts]
WHERE [ContactId] IN
(
    @OwnerId,
    @BuyerId
);