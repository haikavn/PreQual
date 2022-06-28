CREATE TABLE [dbo].[FilterCondition] (
    [FilterId]           BIGINT        NULL,
    [Value]              VARCHAR (150) NULL,
    [Condition]          SMALLINT      NULL,
    [ConditionOperator]  SMALLINT      NULL,
    [Id]                 BIGINT        IDENTITY (1, 1) NOT NULL,
    [CampaignTemplateId] BIGINT        NULL,
    [Value2]             VARCHAR (MAX) NULL,
    [ParentId]           BIGINT        NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);











