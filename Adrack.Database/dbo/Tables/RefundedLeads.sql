CREATE TABLE [dbo].[RefundedLeads] (
    [Id]          BIGINT         IDENTITY (1, 1) NOT NULL,
    [LeadId]      BIGINT         NULL,
    [APrice]      SMALLMONEY     NULL,
    [BPrice]      SMALLMONEY     NULL,
    [DateCreated] DATETIME       NULL,
    [AInvoiceId]  BIGINT         NULL,
    [BInvoiceId]  BIGINT         NULL,
    [Reason]      NVARCHAR (500) NULL,
    [ReviewNote]  NVARCHAR (500) NULL,
    [Approved]    TINYINT        NULL,
    CONSTRAINT [PK_refundedleads] PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([LeadId] ASC)
);






























GO
CREATE NONCLUSTERED INDEX [NIX_RefundedLeads_Approved_BInvoiceId_INCLUDE]
    ON [dbo].[RefundedLeads]([Approved] ASC, [BInvoiceId] ASC)
    INCLUDE([BPrice]) WITH (FILLFACTOR = 90);

