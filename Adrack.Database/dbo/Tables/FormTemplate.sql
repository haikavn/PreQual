CREATE TABLE [dbo].[FormTemplate] (
    [Id]       BIGINT       IDENTITY (1, 1) NOT NULL,
    [Name]     VARCHAR (50) NULL,
    [HtmlCode] TEXT         NULL,
    CONSTRAINT [PK_FormTemplate] PRIMARY KEY CLUSTERED ([Id] ASC)
);

