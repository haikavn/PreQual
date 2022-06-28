CREATE TABLE [dbo].[CampaignTemplate] (
    [Id]                  BIGINT         IDENTITY (1, 1) NOT NULL,
    [CampaignId]          BIGINT         NULL,
    [TemplateField]       VARCHAR (100)  NULL,
    [DatabaseField]       VARCHAR (50)   NULL,
    [Validator]           SMALLINT       NULL,
    [SectionName]         VARCHAR (50)   NULL,
    [Description]         VARCHAR (300)  NULL,
    [MinLength]           INT            NULL,
    [MaxLength]           INT            NULL,
    [Required]            BIT            NULL,
    [BlackListTypeId]     BIGINT         NULL,
    [PossibleValue]       VARCHAR (255)  NULL,
    [IsHash]              BIT            CONSTRAINT [DF_CampaignTemplate_IsHash] DEFAULT ((0)) NULL,
    [IsHidden]            BIT            NULL,
    [ValidatorValue]      VARCHAR (255)  NULL,
    [IsFilterable]        BIT            NULL,
    [Label]               VARCHAR (50)   NULL,
    [ColumnNumber]        INT            NULL,
    [PageNumber]          INT            NULL,
    [IsFormField]         BIT            NULL,
    [OptionValues]        VARCHAR (1000) NULL,
    [FieldType]           SMALLINT       NULL,
    [FieldFilterSettings] VARCHAR (MAX)  NULL,
    CONSTRAINT [PK_CampaignTemplate] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CampaignTemplate_Campaign] FOREIGN KEY ([CampaignId]) REFERENCES [dbo].[Campaign] ([Id])
);


























GO
CREATE NONCLUSTERED INDEX [NIX_CampaignTemplate_CampaignId]
    ON [dbo].[CampaignTemplate]([CampaignId] ASC) WITH (FILLFACTOR = 90);

