CREATE TABLE [dbo].[Authentication] (
    [Id]                BIGINT        IDENTITY (1, 1) NOT NULL,
    [UserId]            BIGINT        NOT NULL,
    [Email]             VARCHAR (100) NULL,
    [Identifier]        VARCHAR (500) NULL,
    [IdentifierDisplay] VARCHAR (500) NULL,
    [OAuthToken]        VARCHAR (500) NULL,
    [OAuthTokenAccess]  VARCHAR (500) NULL,
    [ProviderKey]       VARCHAR (500) NULL,
    CONSTRAINT [PK_Authentication] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Authentication_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);




GO
CREATE NONCLUSTERED INDEX [NIX_Authentication_UserId]
    ON [dbo].[Authentication]([UserId] ASC) WITH (FILLFACTOR = 90);

