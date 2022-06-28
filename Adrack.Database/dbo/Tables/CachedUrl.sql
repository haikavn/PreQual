CREATE TABLE [dbo].[CachedUrl] (
    [Id]  BIGINT        IDENTITY (1, 1) NOT NULL,
    [Url] VARCHAR (500) NULL,
    CONSTRAINT [PK_CachedUrl] PRIMARY KEY CLUSTERED ([Id] ASC)
);



