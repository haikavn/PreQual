CREATE TABLE [dbo].[EmailQueue] (
    [Id]               BIGINT          IDENTITY (1, 1) NOT NULL,
    [SmtpAccountId]    BIGINT          NOT NULL,
    [AttachmentId]     BIGINT          NULL,
    [Sender]           NVARCHAR (200)  NOT NULL,
    [SenderName]       NVARCHAR (100)  NULL,
    [Recipient]        NVARCHAR (200)  NOT NULL,
    [RecipientName]    NVARCHAR (100)  NULL,
    [ReplyTo]          NVARCHAR (200)  NULL,
    [ReplyToName]      NVARCHAR (100)  NULL,
    [Cc]               NVARCHAR (200)  NULL,
    [Bcc]              NVARCHAR (200)  NULL,
    [Subject]          NVARCHAR (1000) NOT NULL,
    [Body]             TEXT            NULL,
    [AttachmentName]   NVARCHAR (200)  NULL,
    [AttachmentPath]   NVARCHAR (400)  NULL,
    [Priority]         INT             NOT NULL,
    [DeliveryAttempts] INT             NOT NULL,
    [SentOn]           DATETIME        NULL,
    [CreatedOn]        DATETIME        CONSTRAINT [DF_EmailQueue_CreatedOn] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_EmailQueue] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_EmailQueue_SmtpAccountId] FOREIGN KEY ([SmtpAccountId]) REFERENCES [dbo].[SmtpAccount] ([Id])
);


















GO
CREATE NONCLUSTERED INDEX [NIX_EmailQueue_SmtpAccountId]
    ON [dbo].[EmailQueue]([SmtpAccountId] ASC) WITH (FILLFACTOR = 90);

