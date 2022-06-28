CREATE TABLE [dbo].[LeadMainReportDayHour] (
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
    [Hour]               INT          NULL,
    [Quantity]           INT          NULL,
    [State]              VARCHAR (50) NULL,
    [CampaignType]       SMALLINT     NULL,
    CONSTRAINT [PK_LeadMainReportDayHour] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_LeadMainReportDayHour_affiliate] FOREIGN KEY ([AffiliateId]) REFERENCES [dbo].[Affiliate] ([Id]),
    CONSTRAINT [FK_LeadMainReportDayHour_affiliatechannel] FOREIGN KEY ([AffiliateChannelId]) REFERENCES [dbo].[AffiliateChannel] ([Id]),
    CONSTRAINT [FK_LeadMainReportDayHour_buyer] FOREIGN KEY ([BuyerId]) REFERENCES [dbo].[Buyer] ([Id]),
    CONSTRAINT [FK_LeadMainReportDayHour_buyerchannel] FOREIGN KEY ([BuyerChannelId]) REFERENCES [dbo].[BuyerChannel] ([Id]),
    CONSTRAINT [FK_LeadMainReportDayHour_campaign] FOREIGN KEY ([CampaignId]) REFERENCES [dbo].[Campaign] ([Id])
);




GO
CREATE NONCLUSTERED INDEX [NIX_Status_Created_Quantity]
    ON [dbo].[LeadMainReportDayHour]([Status] ASC, [Created] ASC)
    INCLUDE([BuyerId], [BuyerChannelId], [Hour], [Quantity]);


GO
CREATE NONCLUSTERED INDEX [NIX_Status_Created]
    ON [dbo].[LeadMainReportDayHour]([Status] ASC, [Created] ASC)
    INCLUDE([BuyerId], [BuyerChannelId], [BuyerPrice], [Hour]);


GO
CREATE NONCLUSTERED INDEX [NIX_GetLeadsCountByHours]
    ON [dbo].[LeadMainReportDayHour]([BuyerChannelId] ASC, [Status] ASC, [Created] ASC, [Hour] ASC);


GO
CREATE NONCLUSTERED INDEX [NIX_Campaign_Created_Status]
    ON [dbo].[LeadMainReportDayHour]([CampaignId] ASC, [Created] ASC, [Status] ASC)
    INCLUDE([BuyerId], [BuyerChannelId], [Hour], [Quantity]);


GO
CREATE NONCLUSTERED INDEX [NIX_BuyerId_BuyerChannelId_Status_Created_Hour_Quantity]
    ON [dbo].[LeadMainReportDayHour]([BuyerId] ASC, [BuyerChannelId] ASC, [Status] ASC, [Created] ASC, [Hour] ASC)
    INCLUDE([Quantity]);


GO
CREATE NONCLUSTERED INDEX [NIX_BuyerId_BuyerChannelId_Status_Created_Hour_Price]
    ON [dbo].[LeadMainReportDayHour]([BuyerId] ASC, [BuyerChannelId] ASC, [Status] ASC, [Created] ASC, [Hour] ASC)
    INCLUDE([BuyerPrice]);


GO
CREATE NONCLUSTERED INDEX [NIX_BuyerChannelId_Status_Created_Hour]
    ON [dbo].[LeadMainReportDayHour]([BuyerChannelId] ASC, [Status] ASC, [Created] ASC, [Hour] ASC);


GO
CREATE NONCLUSTERED INDEX [NIX_AllFields]
    ON [dbo].[LeadMainReportDayHour]([CampaignId] ASC, [BuyerId] ASC, [AffiliateId] ASC, [AffiliateChannelId] ASC, [BuyerChannelId] ASC, [Status] ASC, [Created] ASC, [Hour] ASC, [State] ASC);

