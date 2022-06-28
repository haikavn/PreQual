CREATE TABLE [dbo].[UserSubscribtion] (
    [Id]           BIGINT IDENTITY (1, 1) NOT NULL,
    [UserId]       BIGINT NULL,
    [SubscriberId] BIGINT NULL,
    CONSTRAINT [PK_UserSubscribtion] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_UserSubscribtion_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);

