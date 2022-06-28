CREATE TABLE [dbo].[ArchiveTableList] (
    [Id]   BIGINT        IDENTITY (1, 1) NOT NULL,
    [Name] VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_ArchiveTableList] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

CREATE UNIQUE NONCLUSTERED INDEX [UNIX_ArchiveTableList_Name]
    ON [dbo].[ArchiveTableList]([Name] ASC) WITH (FILLFACTOR = 90);
GO

