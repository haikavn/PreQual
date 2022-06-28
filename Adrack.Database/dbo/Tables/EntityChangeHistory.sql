CREATE TABLE [dbo].[EntityChangeHistory] (
    [Id]           BIGINT        IDENTITY (1, 1) NOT NULL,
    [ModifiedDate] DATETIME      NOT NULL,
    [State]        VARCHAR (50)  NOT NULL,
    [Entity]       VARCHAR (MAX) NOT NULL,
    [EntityId]     BIGINT        NOT NULL,
    [ChangedData]  VARCHAR (MAX) NOT NULL,
    [UserId]       BIGINT        NOT NULL,
    CONSTRAINT [PK_EntityChangeData] PRIMARY KEY CLUSTERED ([Id] ASC)
);

