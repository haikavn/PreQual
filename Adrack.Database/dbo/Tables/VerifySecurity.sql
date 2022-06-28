CREATE TABLE [dbo].[VerifySecurity] (
    [Id]        BIGINT         IDENTITY (1, 1) NOT NULL,
    [UserId]    BIGINT         NOT NULL,
    [Question]  NVARCHAR (150) NOT NULL,
    [Answer]    NVARCHAR (150) NOT NULL,
    [CreatedOn] DATETIME       CONSTRAINT [DF_VerifySecurity_CreatedOn] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_VerifySecurity] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_VerifySecurity_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [NIX_VerifySecurity_UserId]
    ON [dbo].[VerifySecurity]([UserId] ASC) WITH (FILLFACTOR = 90);

