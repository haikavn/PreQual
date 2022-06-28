CREATE TABLE [dbo].[LeadMainReportDay] (
    [Id]                 BIGINT       IDENTITY (1, 1) NOT NULL,
    [CampaignId]         BIGINT       NULL,
    [BuyerId]            BIGINT       NULL,
    [AffiliateId]        BIGINT       NULL,
    [AffiliateChannelId] BIGINT       NULL,
    [BuyerChannelId]     BIGINT       NULL,
    [Status]             SMALLINT     NULL,
    [AffiliatePrice]     MONEY        NULL,
    [BuyerPrice]         MONEY        NULL,
    [Created]            DATE         NULL,
    [Quantity]           INT          NULL,
    [State]              VARCHAR (50) NULL,
    [CampaignType]       SMALLINT     NULL,
    CONSTRAINT [PK_LeadMainReportDay] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_LeadMainReportDay_affiliate] FOREIGN KEY ([AffiliateId]) REFERENCES [dbo].[Affiliate] ([Id]),
    CONSTRAINT [FK_LeadMainReportDay_affiliatechannel] FOREIGN KEY ([AffiliateChannelId]) REFERENCES [dbo].[AffiliateChannel] ([Id]),
    CONSTRAINT [FK_LeadMainReportDay_buyer] FOREIGN KEY ([BuyerId]) REFERENCES [dbo].[Buyer] ([Id]),
    CONSTRAINT [FK_LeadMainReportDay_buyerchannel] FOREIGN KEY ([BuyerChannelId]) REFERENCES [dbo].[BuyerChannel] ([Id]),
    CONSTRAINT [FK_LeadMainReportDay_campaign] FOREIGN KEY ([CampaignId]) REFERENCES [dbo].[Campaign] ([Id])
);














GO
CREATE NONCLUSTERED INDEX [NIX_LeadMainReportDay_CampaignId]
    ON [dbo].[LeadMainReportDay]([CampaignId] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [NIX_LeadMainReportDay_BuyerId]
    ON [dbo].[LeadMainReportDay]([BuyerId] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [NIX_LeadMainReportDay_BuyerChannelId]
    ON [dbo].[LeadMainReportDay]([BuyerChannelId] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [NIX_LeadMainReportDay_AffiliateId]
    ON [dbo].[LeadMainReportDay]([AffiliateId] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [NIX_LeadMainReportDay_AffiliateChannelId]
    ON [dbo].[LeadMainReportDay]([AffiliateChannelId] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [NIX_LeadMainReportDay_BuyerId_BuyerChannelId_Status_Created_INCLUDE]
    ON [dbo].[LeadMainReportDay]([BuyerId] ASC, [BuyerChannelId] ASC, [Status] ASC, [Created] ASC)
    INCLUDE([BuyerPrice], [Quantity]) WITH (FILLFACTOR = 90);

GO
CREATE NONCLUSTERED INDEX [NIX_LeadMainReportDay_BuyerId_Created]
    ON [dbo].[LeadMainReportDay]([BuyerId] ASC, [Created] ASC)
    


GO
CREATE NONCLUSTERED INDEX [NIX_Created_IncludeAll]
    ON [dbo].[LeadMainReportDay]([Created] ASC)
    INCLUDE([CampaignId], [BuyerId], [AffiliateId], [Quantity], [State]);


GO
CREATE NONCLUSTERED INDEX [NIX_Created_Include_Status]
    ON [dbo].[LeadMainReportDay]([Created] ASC)
    INCLUDE([Status]);


GO
CREATE NONCLUSTERED INDEX [NIX_AllFields_Quantity]
    ON [dbo].[LeadMainReportDay]([Status] ASC, [CampaignType] ASC, [Created] ASC)
    INCLUDE([CampaignId], [BuyerId], [AffiliateId], [AffiliatePrice], [BuyerPrice], [Quantity]);

