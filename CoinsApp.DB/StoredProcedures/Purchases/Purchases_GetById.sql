CREATE PROCEDURE [dbo].[Purchases_GetById]
    @PurchaseId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.PurchaseId,
        p.CoinId,

        p.PurchaseDate,
        p.PurchasePrice,

        p.CurrencyId,
        currency.Code AS CurrencyCode,
        currency.Name AS CurrencyName,

        p.SellerId,
        seller.Name AS SellerName,

        p.Notes

    FROM [dbo].[Purchases] AS p

    INNER JOIN [dbo].[Currencies] AS currency
        ON currency.CurrencyId = p.CurrencyId

    LEFT JOIN [dbo].[Contacts] AS seller
        ON seller.ContactId = p.SellerId

    WHERE p.PurchaseId = @PurchaseId;
END;
GO