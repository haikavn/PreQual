CREATE TABLE [dbo].[ProcessingLog] (
    [Id]      BIGINT         IDENTITY (1, 1) NOT NULL,
    [Name]    VARCHAR (100)  NULL,
    [Created] DATETIME       NULL,
    [Message] VARCHAR (1500) NULL,
    [LeadId]  BIGINT         NULL,
    CONSTRAINT [PK_ProcessingLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

