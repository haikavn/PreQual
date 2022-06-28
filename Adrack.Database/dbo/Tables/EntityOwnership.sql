CREATE TABLE [dbo].[EntityOwnership] (
    [Id]         BIGINT       IDENTITY (1, 1) NOT NULL,
    [UserId]     BIGINT       NULL,
    [EntityId]   BIGINT       NULL,
    [EntityName] VARCHAR (50) NULL,
    CONSTRAINT [PK_EntityOwnership] PRIMARY KEY CLUSTERED ([Id] ASC)
);

