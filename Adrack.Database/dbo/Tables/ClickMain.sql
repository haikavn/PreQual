CREATE TABLE [dbo].[ClickMain] (
    [Id]             BIGINT       IDENTITY (1, 1) NOT NULL,
    [CreatedAt]      DATETIME     NULL,
    [ClickChannelId] BIGINT       NULL,
    [ClickType]      SMALLINT     NULL,
    [ClickPrice]     SMALLMONEY   NULL,
    [IpAddress]      VARCHAR (20) NULL,
    CONSTRAINT [PK_ClickMain] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ClickMain_ClickChannel] FOREIGN KEY ([ClickChannelId]) REFERENCES [dbo].[ClickChannel] ([Id])
);



