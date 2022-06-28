CREATE TABLE [dbo].[LeadMainReport] (
    [Id]                 BIGINT       IDENTITY (1, 1) NOT NULL,
    [Created]            DATETIME     NULL,
    [CreatedUtc]         DATETIME     NULL,
    [CampaignId]         BIGINT       NULL,
    [BuyerId]            BIGINT       NULL,
    [AffiliateId]        BIGINT       NULL,
    [Status]             SMALLINT     NULL,
    [AffiliateChannelId] BIGINT       NULL,
    [CampaignType]       SMALLINT     NULL,
    [LeadNumber]         BIGINT       NULL,
    [Warning]            SMALLINT     NULL,
    [ProcessingTime]     FLOAT (53)   NULL,
    [DublicateLeadId]    BIGINT       NULL,
    [AffiliatePrice]     MONEY        NULL,
    [BuyerPrice]         MONEY        NULL,
    [AInvoiceId]         BIGINT       NULL,
    [BInvoiceId]         BIGINT       NULL,
    [ResponseId]         BIGINT       NULL,
    [LeadId]             BIGINT       NULL,
    [ResponseTime]       FLOAT (53)   NULL,
    [State]              VARCHAR (50) NULL,
    [BuyerChannelId]     BIGINT       NULL,
    CONSTRAINT [PK_leads_main] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_LeadMainReport_AffiliateChannel] FOREIGN KEY ([AffiliateChannelId]) REFERENCES [dbo].[AffiliateChannel] ([Id]),
    CONSTRAINT [FK_LeadMainReport_Buyer] FOREIGN KEY ([BuyerId]) REFERENCES [dbo].[Buyer] ([Id]),
    CONSTRAINT [FK_LeadMainReport_BuyerChannel] FOREIGN KEY ([BuyerChannelId]) REFERENCES [dbo].[BuyerChannel] ([Id]),
    CONSTRAINT [FK_LeadMainReport_Campaign] FOREIGN KEY ([CampaignId]) REFERENCES [dbo].[Campaign] ([Id]),
    CONSTRAINT [FK_LeadMainReport_LeadMain] FOREIGN KEY ([LeadId]) REFERENCES [dbo].[LeadMain] ([Id])
);






















GO
CREATE NONCLUSTERED INDEX [NIX_LeadMainReport_LeadId]
    ON [dbo].[LeadMainReport]([LeadId] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [NIX_LeadMainReport_CampaignId]
    ON [dbo].[LeadMainReport]([CampaignId] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [NIX_LeadMainReport_BuyerId]
    ON [dbo].[LeadMainReport]([BuyerId] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [NIX_LeadMainReport_BuyerChannelId]
    ON [dbo].[LeadMainReport]([BuyerChannelId] ASC) WITH (FILLFACTOR = 90);


GO



GO
CREATE NONCLUSTERED INDEX [ix_IndexName]
    ON [dbo].[LeadMainReport]([ResponseId] ASC);


GO
CREATE NONCLUSTERED INDEX [Missing1_IXNC_LeadMainReport_CampaignType_Created_71F57]
    ON [dbo].[LeadMainReport]([CampaignType] ASC, [Created] ASC)
    INCLUDE([BuyerId], [AffiliateId], [LeadId]);


GO



GO
CREATE NONCLUSTERED INDEX [ix_Grigor5invoice]
    ON [dbo].[LeadMainReport]([Status] ASC, [BInvoiceId] ASC)
    INCLUDE([CampaignId], [BuyerPrice]);


GO
CREATE NONCLUSTERED INDEX [ix_Grigor4]
    ON [dbo].[LeadMainReport]([Status] ASC, [Created] ASC)
    INCLUDE([LeadId]);


GO



GO
CREATE NONCLUSTERED INDEX [status]
    ON [dbo].[LeadMainReport]([Status] ASC, [Created] ASC)
    INCLUDE([AffiliatePrice], [BuyerPrice], [LeadId]);


GO



GO



GO
CREATE NONCLUSTERED INDEX [NIX_LeadMainReport_AffiliateId_Status_AInvoiceId_Created_INCLUDE]
    ON [dbo].[LeadMainReport]([AffiliateId] ASC, [Status] ASC, [AInvoiceId] ASC, [Created] ASC)
    INCLUDE([AffiliatePrice]) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [nci_wi_LeadMainReport_C4CE4F38A6C5058920634D0AC73FE981]
    ON [dbo].[LeadMainReport]([CampaignType] ASC, [Created] ASC)
    INCLUDE([AffiliateId], [BuyerId], [CampaignId], [LeadId]);


GO
CREATE NONCLUSTERED INDEX [ix_IndexName_Temp1]
    ON [dbo].[LeadMainReport]([Created] ASC)
    INCLUDE([CampaignId], [BuyerId], [AffiliateId], [LeadId]);


GO
CREATE NONCLUSTERED INDEX [ix_IndexName_2]
    ON [dbo].[LeadMainReport]([Status] ASC, [CampaignType] ASC, [Created] ASC)
    INCLUDE([CampaignId], [BuyerId], [AffiliateId], [AffiliatePrice], [BuyerPrice], [LeadId]);

