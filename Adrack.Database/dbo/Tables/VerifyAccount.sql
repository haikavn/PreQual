CREATE TABLE [dbo].[VerifyAccount] (
    [Id]        BIGINT        IDENTITY (1, 1) NOT NULL,
    [UserId]    BIGINT        NOT NULL,
    [Username]  VARCHAR (50)  NOT NULL,
    [Email]     VARCHAR (100) NOT NULL,
    [Password]  VARCHAR (100) NOT NULL,
    [SaltKey]   VARCHAR (50)  NOT NULL,
    [CreatedOn] DATETIME      CONSTRAINT [DF_VerifyAccount_CreatedOn] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_VerifyAccount] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_VerifyAccount_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [NIX_VerifyAccount_UserId]
    ON [dbo].[VerifyAccount]([UserId] ASC) WITH (FILLFACTOR = 90);

