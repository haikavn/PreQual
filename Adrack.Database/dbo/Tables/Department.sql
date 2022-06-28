CREATE TABLE [dbo].[Department] (
    [Id]   BIGINT       IDENTITY (1, 1) NOT NULL,
    [Name] VARCHAR (50) NULL,
    CONSTRAINT [PK_Department] PRIMARY KEY CLUSTERED ([Id] ASC)
);

