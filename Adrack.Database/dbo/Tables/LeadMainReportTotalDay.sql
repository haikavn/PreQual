CREATE TABLE [dbo].[LeadMainReportTotalDay] (
    [Id]                 BIGINT       IDENTITY (1, 1) NOT NULL,
    [AffiliateId]        BIGINT       NULL,
    [AffiliateChannelId] BIGINT       NULL,
    [BuyerId]            BIGINT       NULL,
    [BuyerChannelId]     BIGINT       NULL,
    [CampaignId]         BIGINT       NULL,
    [State]              VARCHAR (50) NULL,
    [Received]           INT          NULL,
    [Posted]             INT          NULL,
    [Rejected]           INT          NULL,
    [AffiliatePrice]     MONEY        NULL,
    [BuyerPrice]         MONEY        NULL,
    [Redirected]         INT          NULL,
    [Created]            DATE         NULL,
    CONSTRAINT [PK_LeadMainReportTotalDay] PRIMARY KEY CLUSTERED ([Id] ASC)
);

