CREATE TABLE [dbo].[AffiliateInvoice] (
    [Id]          BIGINT     IDENTITY (1, 1) NOT NULL,
    [Number]      BIGINT     NOT NULL,
    [DateFrom]    DATETIME   NULL,
    [DateTo]      DATETIME   NULL,
    [DateCreated] DATETIME   NULL,
    [AffiliateId] BIGINT     NULL,
    [Sum]         FLOAT (53) NULL,
    [Refunded]    FLOAT (53) NULL,
    [Adjustment]  FLOAT (53) NULL,
    [UserId]      BIGINT     NULL,
    [Status]      SMALLINT   NULL,
    [Paid]        FLOAT (53) NULL,
    CONSTRAINT [PK_AffiliateInvoice] PRIMARY KEY CLUSTERED ([Id] ASC)
);



