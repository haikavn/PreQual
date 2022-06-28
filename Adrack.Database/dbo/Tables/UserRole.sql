CREATE TABLE [dbo].[UserRole] (
    [UserId] BIGINT NOT NULL,
    [RoleId] BIGINT NOT NULL,
    CONSTRAINT [PK_UserRole] PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_UserRole_Role] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Role] ([Id]),
    CONSTRAINT [FK_UserRole_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);






GO
CREATE NONCLUSTERED INDEX [NIX_UserRole_RoleId]
    ON [dbo].[UserRole]([RoleId] ASC) WITH (FILLFACTOR = 90);

