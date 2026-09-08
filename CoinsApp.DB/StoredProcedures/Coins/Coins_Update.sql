CREATE PROCEDURE [dbo].[Coins_Update]
    @CoinId INT,
    @CollectionId INT,
    @OwnerId INT = NULL,
    @CountryId INT,
    @CurrencyId INT,
    @DenominationId INT,
    @MintId INT = NULL,
    @MaterialId INT = NULL,
    @Year SMALLINT = NULL,
    @MintMark NVARCHAR(10) = NULL,
    @Fineness DECIMAL(7,4) = NULL,
    @Weight DECIMAL(12,4) = NULL,
    @Diameter DECIMAL(10,4) = NULL,
    @Thickness DECIMAL(10,4) = NULL,
    @Shape NVARCHAR(50) = NULL,
    @Description NVARCHAR(2000) = NULL,
    @Designer NVARCHAR(200) = NULL,
    @Mintage BIGINT = NULL,
    @Condition NVARCHAR(50) = NULL,
    @Grade NVARCHAR(20) = NULL,
    @GradingCompany NVARCHAR(100) = NULL,
    @GradingCertificateNumber NVARCHAR(100) = NULL,
    @CurrentPrice DECIMAL(19,4) = NULL,
    @CurrentPriceCurrencyId INT = NULL,
    @CurrentPriceDate DATETIME2(0) = NULL,
    @Notes NVARCHAR(2000) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE [dbo].[Coins]
    SET
        [CollectionId] = @CollectionId,
        [OwnerId] = @OwnerId,
        [CountryId] = @CountryId,
        [CurrencyId] = @CurrencyId,
        [DenominationId] = @DenominationId,
        [MintId] = @MintId,
        [MaterialId] = @MaterialId,
        [Year] = @Year,
        [MintMark] = @MintMark,
        [Fineness] = @Fineness,
        [Weight] = @Weight,
        [Diameter] = @Diameter,
        [Thickness] = @Thickness,
        [Shape] = @Shape,
        [Description] = @Description,
        [Designer] = @Designer,
        [Mintage] = @Mintage,
        [Condition] = @Condition,
        [Grade] = @Grade,
        [GradingCompany] = @GradingCompany,
        [GradingCertificateNumber] = @GradingCertificateNumber,
        [CurrentPrice] = @CurrentPrice,
        [CurrentPriceCurrencyId] = @CurrentPriceCurrencyId,
        [CurrentPriceDate] = @CurrentPriceDate,
        [Notes] = @Notes
    WHERE [CoinId] = @CoinId;

    IF @@ROWCOUNT = 0
    BEGIN
        THROW 50001, 'Coin not found.', 1;
    END;

    SELECT [CoinId]
    FROM [dbo].[Coins]
    WHERE [CoinId] = @CoinId;
END;
GO