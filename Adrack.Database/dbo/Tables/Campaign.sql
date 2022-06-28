CREATE TABLE [dbo].[Campaign] (
    [Id]                    BIGINT           IDENTITY (1, 1) NOT NULL,
    [CreatedOn]             DATETIME         NOT NULL,
    [Name]                  VARCHAR (50)     NULL,
    [Start]                 DATETIME         NULL,
    [Finish]                DATETIME         NULL,
    [Status]                SMALLINT         NULL,
    [Description]           VARCHAR (300)    NULL,
    [CampaignType]          SMALLINT         NULL,
    [PriceFormat]           SMALLINT         NULL,
    [XmlTemplate]           VARCHAR (MAX)    NULL,
    [VerticalId]            BIGINT           NULL,
    [Visibility]            SMALLINT         NULL,
    [IsTemplate]            BIT              NULL,
    [CampaignKey]           VARCHAR (50)     NULL,
    [NetworkTargetRevenue]  SMALLMONEY       NULL,
    [NetworkMinimumRevenue] SMALLMONEY       NULL,
    [Deleted]               BIT              NULL,
    [HtmlFormId]            UNIQUEIDENTIFIER NULL,
    [PingTreeCycle]         SMALLINT         NULL,
    [PrioritizedEnabled]    BIT              NULL,
    CONSTRAINT [PK_Campaign] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Campaign_Vertical] FOREIGN KEY ([VerticalId]) REFERENCES [dbo].[Vertical] ([Id])
);


GO
ALTER TABLE [dbo].[Campaign] NOCHECK CONSTRAINT [FK_Campaign_Vertical];








































GO
CREATE NONCLUSTERED INDEX [NIX_Campaign_VerticalId]
    ON [dbo].[Campaign]([VerticalId] ASC) WITH (FILLFACTOR = 90);

