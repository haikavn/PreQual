CREATE TABLE [dbo].[PaymentMethod] (
    [Id]                  BIGINT         IDENTITY (1, 1) NOT NULL,
    [AffiliateId]         BIGINT         NULL,
    [PaymentType]         SMALLINT       NULL,
    [NameOnAccount]       VARCHAR (50)   NULL,
    [BankName]            VARCHAR (50)   NULL,
    [AccountNumber]       VARCHAR (50)   NULL,
    [SwiftRoutingNumber]  VARCHAR (50)   NULL,
    [BankAddress]         VARCHAR (50)   NULL,
    [BankPhone]           VARCHAR (50)   NULL,
    [AccountOwnerAddress] VARCHAR (50)   NULL,
    [AccountOwnerPhone]   VARCHAR (50)   NULL,
    [SpecialInstructions] VARCHAR (1000) NULL,
    [IsPrimary]           BIT            NULL,
    CONSTRAINT [PK_PaymentMethod] PRIMARY KEY CLUSTERED ([Id] ASC)
);



