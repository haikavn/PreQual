CREATE TABLE [dbo].[EmailTemplate] (
    [Id]            BIGINT          IDENTITY (1, 1) NOT NULL,
    [SmtpAccountId] BIGINT          NOT NULL,
    [AttachmentId]  BIGINT          NULL,
    [Name]          VARCHAR (200)   NOT NULL,
    [Bcc]           VARCHAR (200)   NULL,
    [Subject]       VARCHAR(1000) NOT NULL,
    [Body]          VARCHAR(MAX) NOT NULL,
    [Active]        BIT             NOT NULL,
    CONSTRAINT [PK_EmailTemplate] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_EmailTemplate_SmtpAccountId] FOREIGN KEY ([SmtpAccountId]) REFERENCES [dbo].[SmtpAccount] ([Id])
);
















GO
CREATE NONCLUSTERED INDEX [NIX_EmailTemplate_SmtpAccountId]
    ON [dbo].[EmailTemplate]([SmtpAccountId] ASC) WITH (FILLFACTOR = 90);

