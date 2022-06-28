CREATE TABLE [dbo].[BlackListType] (
    [Id]        BIGINT       IDENTITY (1, 1) NOT NULL,
    [Name]      VARCHAR (50) NULL,
    [BlackType] SMALLINT     NULL,
    [ParentId]  BIGINT       NULL,
    CONSTRAINT [PK_BlackListType] PRIMARY KEY CLUSTERED ([Id] ASC)
);



