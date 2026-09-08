CREATE PROCEDURE [dbo].[Coins_Create]
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
    @Notes NVARCHAR(2000) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [dbo].[Coins]
    (
        [CollectionId],
        [OwnerId],
        [InitialOwnerId],
        [CountryId],
        [CurrencyId],
        [DenominationId],
        [MintId],
        [MaterialId],
        [Year],
        [MintMark],
        [Fineness],
        [Weight],
        [Diameter],
        [Thickness],
        [Shape],
        [Description],
        [Designer],
        [Mintage],
        [Condition],
        [Grade],
        [GradingCompany],
        [GradingCertificateNumber],
        [Notes]
    )
    VALUES
    (
        @CollectionId,
        @OwnerId,
        @OwnerId,
        @CountryId,
        @CurrencyId,
        @DenominationId,
        @MintId,
        @MaterialId,
        @Year,
        @MintMark,
        @Fineness,
        @Weight,
        @Diameter,
        @Thickness,
        @Shape,
        @Description,
        @Designer,
        @Mintage,
        @Condition,
        @Grade,
        @GradingCompany,
        @GradingCertificateNumber,
        @Notes
    );

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS CoinId;
END;
GO