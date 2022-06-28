CREATE TABLE [dbo].[UserPermission] (
    [UserId]       BIGINT NOT NULL,
    [PermissionId] BIGINT NOT NULL,
    CONSTRAINT [PK_UserPermission] PRIMARY KEY CLUSTERED ([UserId] ASC, [PermissionId] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_UserPermission_Permission] FOREIGN KEY ([PermissionId]) REFERENCES [dbo].[Permission] ([Id]),
    CONSTRAINT [FK_UserPermission_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [NIX_UserPermission_PermissionId]
    ON [dbo].[UserPermission]([PermissionId] ASC) WITH (FILLFACTOR = 90);

