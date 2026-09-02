CREATE PROCEDURE [dbo].[PriceHistory_GetByCoin]
    @CoinId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ph.PriceHistoryId,
        ph.CoinId,
        ph.Price,
        ph.CurrencyId,
        currency.Code AS CurrencyCode,
        currency.Name AS CurrencyName,
        ph.PriceDate,
        ph.Source,
        ph.Notes

    FROM [dbo].[PriceHistory] AS ph

    INNER JOIN [dbo].[Currencies] AS currency
        ON currency.CurrencyId = ph.CurrencyId

    WHERE ph.CoinId = @CoinId

    ORDER BY
        ph.PriceDate DESC,
        ph.PriceHistoryId DESC;
END;
GO