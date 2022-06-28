CREATE TABLE [dbo].[BuyerInvoiceAdjustment] (
    [Id]             BIGINT        IDENTITY (1, 1) NOT NULL,
    [BuyerInvoiceId] BIGINT        NULL,
    [Name]           VARCHAR (255) NULL,
    [Price]          FLOAT (53)    NULL,
    [Qty]            INT           NULL,
    [Sum]            FLOAT (53)    NULL,
    CONSTRAINT [PK_BuyerInvoiceAdjustment] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_BuyerInvoiceAdjustment_BuyerInvoice] FOREIGN KEY ([BuyerInvoiceId]) REFERENCES [dbo].[BuyerInvoice] ([Id])
);






GO
CREATE NONCLUSTERED INDEX [NIX_BuyerInvoiceAdjustment_BuyerInvoiceId]
    ON [dbo].[BuyerInvoiceAdjustment]([BuyerInvoiceId] ASC) WITH (FILLFACTOR = 90);

