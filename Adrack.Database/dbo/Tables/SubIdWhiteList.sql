CREATE TABLE [dbo].[SubIdWhiteList] (
    [Id]             BIGINT       IDENTITY (1, 1) NOT NULL,
    [SubId]          VARCHAR (20) NULL,
    [BuyerChannelId] BIGINT       NULL,
    CONSTRAINT [PK_SubIdWhiteList] PRIMARY KEY CLUSTERED ([Id] ASC)
);




GO
CREATE NONCLUSTERED INDEX [SubIdWhiteList_SubID_IX]
    ON [dbo].[SubIdWhiteList]([SubId] ASC);

