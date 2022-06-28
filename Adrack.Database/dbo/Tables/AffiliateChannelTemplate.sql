CREATE TABLE [dbo].[AffiliateChannelTemplate] (
    [Id]                 BIGINT        IDENTITY (1, 1) NOT NULL,
    [CampaignTemplateId] BIGINT        NULL,
    [TemplateField]      VARCHAR (100) NULL,
    [SectionName]        VARCHAR (50)  NULL,
    [AffiliateChannelId] BIGINT        NULL,
    [DefaultValue]       VARCHAR (150) NULL,
    CONSTRAINT [PK_AffiliateChannelTemplate] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AffiliateChannelTemplate_AffiliateChannel] FOREIGN KEY ([AffiliateChannelId]) REFERENCES [dbo].[AffiliateChannel] ([Id])
);










GO
CREATE NONCLUSTERED INDEX [NIX_AffiliateChannelTemplate_AffiliateChannelId]
    ON [dbo].[AffiliateChannelTemplate]([AffiliateChannelId] ASC) WITH (FILLFACTOR = 90);

