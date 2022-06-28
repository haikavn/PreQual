CREATE TABLE [dbo].[BuyerChannelFilterCondition] (
    [Value]              VARCHAR (MAX)  NULL,
    [Condition]          SMALLINT       NULL,
    [ConditionOperator]  SMALLINT       NULL,
    [Id]                 BIGINT         IDENTITY (1, 1) NOT NULL,
    [CampaignTemplateId] BIGINT         NULL,
    [BuyerChannelId]     BIGINT         NULL,
    [Value2]             VARCHAR (1500) NULL,
    [ParentId]           BIGINT         NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_BuyerChannelFilterCondition_BuyerChannel] FOREIGN KEY ([BuyerChannelId]) REFERENCES [dbo].[BuyerChannel] ([Id])
);












GO
CREATE NONCLUSTERED INDEX [NIX_BuyerChannelFilterCondition_BuyerChannelId]
    ON [dbo].[BuyerChannelFilterCondition]([BuyerChannelId] ASC) WITH (FILLFACTOR = 90);

