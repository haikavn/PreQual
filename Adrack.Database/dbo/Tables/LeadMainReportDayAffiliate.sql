CREATE TABLE [dbo].[LeadMainReportDayAffiliate] (
    [Id]                 BIGINT       IDENTITY (1, 1) NOT NULL,
    [AffiliateId]        BIGINT       NULL,
    [AffiliateChannelId] BIGINT       NULL,
    [Created]            DATETIME     NULL,
    [Quantity]           INT          NULL,
    [Status]             SMALLINT     NULL,
    [State]              VARCHAR (50) NULL,
    [CampaignId]         BIGINT       NULL,
    [CampaignType]       SMALLINT     NULL,
    [LeadId]             BIGINT       NULL,
    CONSTRAINT [PK_LeadMainReportDayAffiliate] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_LeadMainReportDayAffiliate_Affiliate] FOREIGN KEY ([AffiliateId]) REFERENCES [dbo].[Affiliate] ([Id]),
    CONSTRAINT [FK_LeadMainReportDayAffiliate_AffiliateChannel] FOREIGN KEY ([AffiliateChannelId]) REFERENCES [dbo].[AffiliateChannel] ([Id]),
    CONSTRAINT [FK_LeadMainReportDayAffiliate_Campaign] FOREIGN KEY ([CampaignId]) REFERENCES [dbo].[Campaign] ([Id])
);






GO
CREATE NONCLUSTERED INDEX [NIX_LeadMainReportDayAffiliate_AffiliateId]
    ON [dbo].[LeadMainReportDayAffiliate]([AffiliateId] ASC);


GO
CREATE NONCLUSTERED INDEX [NIX_LeadMainReportDayAffiliate_AffiliateChannelId]
    ON [dbo].[LeadMainReportDayAffiliate]([AffiliateChannelId] ASC);


GO
CREATE NONCLUSTERED INDEX [NIX_LeadMainReportDayAffiliate_CampaignId]
    ON [dbo].[LeadMainReportDayAffiliate]([CampaignId] ASC);


GO
CREATE NONCLUSTERED INDEX [nci_wi_LeadMainReportDayAffiliate_50158AEF842BAA8C9FBBDEC66F031F06]
    ON [dbo].[LeadMainReportDayAffiliate]([AffiliateChannelId] ASC, [AffiliateId] ASC, [CampaignId] ASC, [Created] ASC, [Status] ASC)
    INCLUDE([CampaignType], [Quantity], [State]);

