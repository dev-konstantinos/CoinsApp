CREATE PROCEDURE [dbo].[Sales_GetByCoin]
    @CoinId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        s.SaleId,
        s.CoinId,

        s.SaleDate,
        s.SalePrice,

        s.CurrencyId,
        currency.Code AS CurrencyCode,
        currency.Name AS CurrencyName,

        s.BuyerId,
        buyer.Name AS BuyerName,

        s.Notes

    FROM [dbo].[Sales] AS s

    INNER JOIN [dbo].[Currencies] AS currency
        ON currency.CurrencyId = s.CurrencyId

    LEFT JOIN [dbo].[Contacts] AS buyer
        ON buyer.ContactId = s.BuyerId

    WHERE s.CoinId = @CoinId

    ORDER BY
        s.SaleDate DESC,
        s.SaleId DESC;
END;
GO