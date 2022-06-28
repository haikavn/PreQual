CREATE TABLE [dbo].[Acl] (
    [Id]         BIGINT        IDENTITY (1, 1) NOT NULL,
    [RoleId]     BIGINT        NOT NULL,
    [EntityId]   BIGINT        NOT NULL,
    [EntityName] VARCHAR (150) NOT NULL,
    CONSTRAINT [PK_Acl] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_Acl_Role] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Role] ([Id])
);








GO
CREATE NONCLUSTERED INDEX [NIX_Acl_RoleId]
    ON [dbo].[Acl]([RoleId] ASC) WITH (FILLFACTOR = 90);

