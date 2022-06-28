CREATE TABLE [dbo].[LeadMain] (
    [Id]                 BIGINT        IDENTITY (1, 1) NOT NULL,
    [Created]            DATETIME      NULL,
    [CampaignId]         BIGINT        NULL,
    [AffiliateId]        BIGINT        NULL,
    [Status]             SMALLINT      NULL,
    [AffiliateChannelId] BIGINT        NULL,
    [CampaignType]       SMALLINT      NULL,
    [LeadNumber]         BIGINT        NULL,
    [Warning]            SMALLINT      NULL,
    [ProcessingTime]     FLOAT (53)    NULL,
    [DublicateLeadId]    BIGINT        NULL,
    [ReceivedData]       VARCHAR (MAX) NULL,
    [BuyerChannelId]     BIGINT        NULL,
    [ErrorType]          SMALLINT      NULL,
    [ViewDate]           DATETIME      NULL,
    [SoldDate]           DATETIME      NULL,
    [CreateArchiveDT]    DATETIME      CONSTRAINT [DF_LeadMain_CreateArchiveDT] DEFAULT (getutcdate()) NOT NULL,
    [UpdateDate]         DATETIME      NULL,
    [RealIp]             VARCHAR (50)  NULL,
    [RiskScore]          INT           NULL,
    CONSTRAINT [PK_leads] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_LeadMain_BuyerChannel] FOREIGN KEY ([BuyerChannelId]) REFERENCES [dbo].[BuyerChannel] ([Id])
);
















































GO



GO
CREATE NONCLUSTERED INDEX [NIX_LeadMain_Include]
    ON [dbo].[LeadMain]([Status] ASC, [AffiliateId] ASC, [AffiliateChannelId] ASC, [CampaignId] ASC)
    INCLUDE([Created]);


GO

GO
CREATE NONCLUSTERED INDEX [ix_createdGrigor]
    ON [dbo].[LeadMain]([Created] ASC)
    INCLUDE([Id]);


GO
CREATE NONCLUSTERED INDEX [Missing_IXNC_LeadMain_Created_RiskScore_09F0A]
    ON [dbo].[LeadMain]([Created] ASC, [RiskScore] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_IndexName_070318]
    ON [dbo].[LeadMain]([Created] ASC)
    INCLUDE([CampaignId], [AffiliateId], [Status], [AffiliateChannelId]);


GO
CREATE NONCLUSTERED INDEX [ix_IndexName]
    ON [dbo].[LeadMain]([Created] ASC)
    INCLUDE([Status]);


GO
CREATE NONCLUSTERED INDEX [LeadMain_CampaignType_AffId]
    ON [dbo].[LeadMain]([CampaignType] ASC, [Created] ASC)
    INCLUDE([AffiliateId]);

