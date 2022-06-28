CREATE TABLE [dbo].[RolePermission] (
    [RoleId]       BIGINT NOT NULL,
    [PermissionId] BIGINT NOT NULL,
    [State] TINYINT NOT NULL, 
    CONSTRAINT [PK_RolePermission] PRIMARY KEY CLUSTERED ([RoleId] ASC, [PermissionId] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_RolePermission_Permission] FOREIGN KEY ([PermissionId]) REFERENCES [dbo].[Permission] ([Id]),
    CONSTRAINT [FK_RolePermission_Role] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Role] ([Id])
);






GO
CREATE NONCLUSTERED INDEX [NIX_RolePermission_PermissionId]
    ON [dbo].[RolePermission]([PermissionId] ASC) WITH (FILLFACTOR = 90);

