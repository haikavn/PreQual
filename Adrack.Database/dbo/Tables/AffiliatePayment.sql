CREATE TABLE [dbo].[AffiliatePayment] (
    [Id]          BIGINT        IDENTITY (1, 1) NOT NULL,
    [AffiliateId] BIGINT        NOT NULL,
    [PaymentDate] DATETIME      NULL,
    [Amount]      FLOAT (53)    NULL,
    [Note]        VARCHAR (255) NULL,
    [Created]     DATETIME      NULL,
    [UserId]      BIGINT        NOT NULL,
    CONSTRAINT [PK_Affiliate_Payment] PRIMARY KEY CLUSTERED ([Id] ASC)
);

