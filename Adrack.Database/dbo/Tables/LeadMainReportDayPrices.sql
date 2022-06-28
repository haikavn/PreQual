CREATE TABLE [dbo].[LeadMainReportDayPrices] (
    [Id]             BIGINT   IDENTITY (1, 1) NOT NULL,
    [CampaignId]     BIGINT   NULL,
    [BuyerId]        BIGINT   NULL,
    [BuyerChannelId] BIGINT   NULL,
    [BuyerPrice]     MONEY    NULL,
    [Created]        DATE     NULL,
    [Quantity]       INT      NULL,
    [UQuantity]      INT      NULL,
    [CampaignType]   SMALLINT NULL,
    [Status]         SMALLINT NULL,
    CONSTRAINT [PK_LeadMainReportDayPrices] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_LeadMainReportDayPrices_buyer] FOREIGN KEY ([BuyerId]) REFERENCES [dbo].[Buyer] ([Id]),
    CONSTRAINT [FK_LeadMainReportDayPrices_buyerchannel] FOREIGN KEY ([BuyerChannelId]) REFERENCES [dbo].[BuyerChannel] ([Id]),
    CONSTRAINT [FK_LeadMainReportDayPrices_campaign] FOREIGN KEY ([CampaignId]) REFERENCES [dbo].[Campaign] ([Id])
);






GO
CREATE NONCLUSTERED INDEX [NIX_LeadMainReportDayPrices_AllFields]
    ON [dbo].[LeadMainReportDayPrices]([CampaignId] ASC, [BuyerId] ASC, [BuyerChannelId] ASC, [BuyerPrice] ASC, [Created] ASC, [Status] ASC);

