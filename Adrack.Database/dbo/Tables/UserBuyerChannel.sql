CREATE TABLE [dbo].[UserBuyerChannel] (
    [Id]             BIGINT IDENTITY (1, 1) NOT NULL,
    [BuyerChannelId] BIGINT NULL,
    [UserId]         BIGINT NULL,
    CONSTRAINT [PK_UserBuyerChannel] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_UserBuyerChannel_BuyerChannel] FOREIGN KEY ([BuyerChannelId]) REFERENCES [dbo].[BuyerChannel] ([Id]),
    CONSTRAINT [FK_UserBuyerChannel_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);





