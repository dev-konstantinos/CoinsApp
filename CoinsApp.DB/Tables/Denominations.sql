CREATE TABLE [dbo].[Denominations]
(
    [DenominationId] INT IDENTITY(1,1) NOT NULL,
    [CurrencyId] INT NOT NULL,
    [Value] DECIMAL(18,4) NOT NULL,
    [DisplayName] NVARCHAR(50) NOT NULL,
    [IsActive] BIT NOT NULL
        CONSTRAINT [DF_Denominations_IsActive] DEFAULT (1),

    CONSTRAINT [PK_Denominations]
        PRIMARY KEY ([DenominationId]),

    CONSTRAINT [FK_Denominations_Currencies]
        FOREIGN KEY ([CurrencyId])
        REFERENCES [dbo].[Currencies] ([CurrencyId]),

    CONSTRAINT [UQ_Denominations_Currency_Value]
        UNIQUE ([CurrencyId], [Value])
);