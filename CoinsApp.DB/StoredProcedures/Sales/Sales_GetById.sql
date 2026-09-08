CREATE PROCEDURE [dbo].[Sales_GetById]
    @SaleId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        s.[SaleId],
        s.[CoinId],
        s.[SaleDate],
        s.[SalePrice],

        s.[CurrencyId],
        cur.[Code] AS [CurrencyCode],
        cur.[Name] AS [CurrencyName],

        s.[PreviousOwnerId],
        previousOwner.[Name] AS [PreviousOwnerName],

        s.[BuyerId],
        buyer.[Name] AS [BuyerName],

        s.[Notes]

    FROM [dbo].[Sales] AS s

    INNER JOIN [dbo].[Currencies] AS cur
        ON cur.[CurrencyId] = s.[CurrencyId]

    LEFT JOIN [dbo].[Contacts] AS previousOwner
        ON previousOwner.[ContactId] = s.[PreviousOwnerId]

    LEFT JOIN [dbo].[Contacts] AS buyer
        ON buyer.[ContactId] = s.[BuyerId]

    WHERE s.[SaleId] = @SaleId;
END;
GO