CREATE TABLE [dbo].[BuyerChannelTemplate] (
    [Id]                 BIGINT        IDENTITY (1, 1) NOT NULL,
    [CampaignTemplateId] BIGINT        NULL,
    [TemplateField]      VARCHAR (100) NULL,
    [SectionName]        VARCHAR (150) NULL,
    [BuyerChannelId]     BIGINT        NULL,
    [DefaultValue]       VARCHAR (150) NULL,
    CONSTRAINT [PK_BuyerChannelTemplate] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_BuyerChannelTemplate_BuyerChannel] FOREIGN KEY ([BuyerChannelId]) REFERENCES [dbo].[BuyerChannel] ([Id])
);










GO
CREATE NONCLUSTERED INDEX [NIX_BuyerChannelTemplate_BuyerChannelId]
    ON [dbo].[BuyerChannelTemplate]([BuyerChannelId] ASC) WITH (FILLFACTOR = 90);

