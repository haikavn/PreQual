CREATE TABLE [dbo].[NoteTitle] (
    [Id]    BIGINT        IDENTITY (1, 1) NOT NULL,
    [Title] VARCHAR (255) NOT NULL,
    CONSTRAINT [PK_NoteTitle] PRIMARY KEY CLUSTERED ([Id] ASC)
);

