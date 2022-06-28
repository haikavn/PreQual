CREATE TABLE [dbo].[LeadMainReportDayRedirected] (
    [Id]                 BIGINT       IDENTITY (1, 1) NOT NULL,
    [AffiliateId]        BIGINT       NULL,
    [AffiliateChannelId] BIGINT       NULL,
    [Created]            DATETIME     NULL,
    [Quantity]           INT          NULL,
    [CampaignId]         BIGINT       NULL,
    [CampaignType]       SMALLINT     NULL,
    [BuyerId]            BIGINT       NULL,
    [BuyerChannelId]     BIGINT       NULL,
    [State]              VARCHAR (50) NULL,
    CONSTRAINT [PK_LeadMainReportDayRedirected] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_LeadMainReportDayRedirected_Affiliate] FOREIGN KEY ([AffiliateId]) REFERENCES [dbo].[Affiliate] ([Id]),
    CONSTRAINT [FK_LeadMainReportDayRedirected_AffiliateChannel] FOREIGN KEY ([AffiliateChannelId]) REFERENCES [dbo].[AffiliateChannel] ([Id]),
    CONSTRAINT [FK_LeadMainReportDayRedirected_Campaign] FOREIGN KEY ([CampaignId]) REFERENCES [dbo].[Campaign] ([Id])
);



