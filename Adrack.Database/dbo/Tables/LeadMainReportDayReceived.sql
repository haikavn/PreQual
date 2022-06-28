CREATE TABLE [dbo].[LeadMainReportDayReceived] (
    [Id]                 BIGINT   IDENTITY (1, 1) NOT NULL,
    [AffiliateId]        BIGINT   NULL,
    [AffiliateChannelId] BIGINT   NULL,
    [Created]            DATETIME NULL,
    [Quantity]           INT      NULL,
    [CampaignId]         BIGINT   NULL,
    [CampaignType]       SMALLINT NULL,
    [BuyerId]            BIGINT   NULL,
    [BuyerChannelId]     BIGINT   NULL,
    [LeadId]             BIGINT   NULL,
    CONSTRAINT [PK_LeadMainReportDayReceived] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_LeadMainReportDayReceived_Affiliate] FOREIGN KEY ([AffiliateId]) REFERENCES [dbo].[Affiliate] ([Id]),
    CONSTRAINT [FK_LeadMainReportDayReceived_AffiliateChannel] FOREIGN KEY ([AffiliateChannelId]) REFERENCES [dbo].[AffiliateChannel] ([Id]),
    CONSTRAINT [FK_LeadMainReportDayReceived_Campaign] FOREIGN KEY ([CampaignId]) REFERENCES [dbo].[Campaign] ([Id])
);







