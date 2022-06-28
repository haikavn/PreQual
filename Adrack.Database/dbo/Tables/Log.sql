CREATE TABLE [dbo].[Log] (
    [Id]          BIGINT          IDENTITY (1, 1) NOT NULL,
    [UserId]      BIGINT          NULL,
    [LogLevelId]  INT             NOT NULL,
    [Message]     NVARCHAR (2000) NOT NULL,
    [Exception]   NVARCHAR (MAX)  NULL,
    [IpAddress]   VARCHAR (15)    NULL,
    [PageUrl]     VARCHAR (5000)  NULL,
    [ReferrerUrl] VARCHAR (5000)  NULL,
    [CreatedOn]   DATETIME        CONSTRAINT [DF_Log_CreatedOn] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_Log_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id]) ON DELETE CASCADE
);






















GO
CREATE NONCLUSTERED INDEX [NIX_Log_UserId]
    ON [dbo].[Log]([UserId] ASC) WITH (FILLFACTOR = 90);

