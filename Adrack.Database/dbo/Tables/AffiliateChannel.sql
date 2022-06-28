CREATE TABLE [dbo].[AffiliateChannel] (
    [Id]                   BIGINT        IDENTITY (1, 1) NOT NULL,
    [CampaignId]           BIGINT        NULL,
    [XmlTemplate]          VARCHAR (MAX) NULL,
    [AffiliateId]          BIGINT        NULL,
    [Name]                 VARCHAR (50)  NULL,
    [Status]               SMALLINT      NULL,
    [DataFormat]           SMALLINT      NULL,
    [MinPriceOption]       SMALLINT      NULL,
    [MinPriceOptionValue]  SMALLMONEY    NULL,
    [MinRevenue]           SMALLMONEY    NULL,
    [AffiliateChannelKey]  VARCHAR (50)  NULL,
    [Deleted]              BIT           NULL,
    [AffiliatePriceMethod] SMALLINT      NULL,
    [AffiliatePrice]       SMALLMONEY    NULL,
    [Timeout]              SMALLINT      NULL,
    [Note]                 VARCHAR (MAX) NULL,
    CONSTRAINT [PK_AffiliateChannel] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AffiliateChannel_Affiliate] FOREIGN KEY ([AffiliateId]) REFERENCES [dbo].[Affiliate] ([Id]),
    CONSTRAINT [FK_AffiliateChannel_Campaign] FOREIGN KEY ([CampaignId]) REFERENCES [dbo].[Campaign] ([Id])
);


























GO
CREATE NONCLUSTERED INDEX [NIX_AffiliateChannel_CampaignId]
    ON [dbo].[AffiliateChannel]([CampaignId] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [NIX_AffiliateChannel_AffiliateId]
    ON [dbo].[AffiliateChannel]([AffiliateId] ASC) WITH (FILLFACTOR = 90);

