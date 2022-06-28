CREATE TABLE [dbo].[BuyerInvoice] (
    [Id]          BIGINT     IDENTITY (1, 1) NOT NULL,
    [Number]      BIGINT     NOT NULL,
    [DateFrom]    DATETIME   NULL,
    [DateTo]      DATETIME   NULL,
    [DateCreated] DATETIME   NULL,
    [BuyerId]     BIGINT     NULL,
    [Sum]         FLOAT (53) NULL,
    [Refunded]    FLOAT (53) NULL,
    [Adjustment]  FLOAT (53) NULL,
    [UserId]      BIGINT     NULL,
    [Status]      SMALLINT   NULL,
    [Paid]        FLOAT (53) NULL,
    CONSTRAINT [PK_BuyerInvoice] PRIMARY KEY CLUSTERED ([Id] ASC)
);








GO
CREATE NONCLUSTERED INDEX [NIX_BuyerInvoice_BuyerId_INCLUDE]
    ON [dbo].[BuyerInvoice]([BuyerId] ASC)
    INCLUDE([Number]) WITH (FILLFACTOR = 90);

