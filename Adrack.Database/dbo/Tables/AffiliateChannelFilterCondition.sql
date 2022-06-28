CREATE TABLE [dbo].[AffiliateChannelFilterCondition] (
    [Value]              VARCHAR (MAX) NULL,
    [Condition]          SMALLINT      NULL,
    [ConditionOperator]  SMALLINT      NULL,
    [Id]                 BIGINT        IDENTITY (1, 1) NOT NULL,
    [CampaignTemplateId] BIGINT        NULL,
    [AffiliateChannelId] BIGINT        NULL,
    [ParentId]           BIGINT        NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AffiliateChannelFilterCondition_AffiliateChannel] FOREIGN KEY ([AffiliateChannelId]) REFERENCES [dbo].[AffiliateChannel] ([Id])
);








GO
CREATE NONCLUSTERED INDEX [NIX_AffiliateChannelFilterCondition_AffiliateChannelId]
    ON [dbo].[AffiliateChannelFilterCondition]([AffiliateChannelId] ASC) WITH (FILLFACTOR = 90);

