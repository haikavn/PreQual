CREATE TABLE [dbo].[AffiliateInvoiceAdjustment] (
    [Id]                 BIGINT        IDENTITY (1, 1) NOT NULL,
    [AffiliateInvoiceId] BIGINT        NULL,
    [Name]               VARCHAR (255) NULL,
    [Price]              FLOAT (53)    NULL,
    [Qty]                INT           NULL,
    [Sum]                FLOAT (53)    NULL,
    CONSTRAINT [PK_AffiliateInvoiceAdjustment] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AffiliateInvoiceAdjustment_AffiliateInvoice] FOREIGN KEY ([AffiliateInvoiceId]) REFERENCES [dbo].[AffiliateInvoice] ([Id])
);






GO
CREATE NONCLUSTERED INDEX [NIX_AffiliateInvoiceAdjustment_AffiliateInvoiceId]
    ON [dbo].[AffiliateInvoiceAdjustment]([AffiliateInvoiceId] ASC) WITH (FILLFACTOR = 90);

