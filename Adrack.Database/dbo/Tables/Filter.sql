CREATE TABLE [dbo].[Filter] (
    [Id]         BIGINT       IDENTITY (1, 1) NOT NULL,
    [Name]       VARCHAR (50) NULL,
    [CampaignId] BIGINT       NULL,
    [VerticalId] BIGINT       NULL,
    CONSTRAINT [PK_Filter] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Filter_Campaign] FOREIGN KEY ([CampaignId]) REFERENCES [dbo].[Campaign] ([Id]),
    CONSTRAINT [FK_Filter_Vertical] FOREIGN KEY ([VerticalId]) REFERENCES [dbo].[Vertical] ([Id])
);












GO
CREATE NONCLUSTERED INDEX [NIX_Filter_VerticalId]
    ON [dbo].[Filter]([VerticalId] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [NIX_Filter_CampaignId]
    ON [dbo].[Filter]([CampaignId] ASC) WITH (FILLFACTOR = 90);

