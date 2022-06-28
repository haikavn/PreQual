CREATE TABLE [dbo].[ArchiveTableLog] (
    [Id]        BIGINT         IDENTITY (1, 1) NOT NULL,
    [Name]      VARCHAR (300)  NOT NULL,
    [Message]   VARCHAR (2000) NOT NULL,
    [Exception] VARCHAR (4000) NULL,
    [CreatedOn] DATETIME       CONSTRAINT [DF_ArchiveTableLog_CreatedOn] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_ArchiveTableLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);



