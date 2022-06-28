CREATE TABLE [dbo].[ClickChannel] (
    [Id]                 BIGINT        IDENTITY (1, 1) NOT NULL,
    [Name]               VARCHAR (50)  NULL,
    [AffiliateChannelId] BIGINT        NULL,
    [ClickPrice]         SMALLMONEY    NULL,
    [AccessKey]          VARCHAR (150) NULL,
    CONSTRAINT [PK_ClickChannel] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ClickChannel_AffiliateChannel] FOREIGN KEY ([AffiliateChannelId]) REFERENCES [dbo].[AffiliateChannel] ([Id])
);

