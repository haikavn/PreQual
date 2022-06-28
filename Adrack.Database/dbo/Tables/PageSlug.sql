CREATE TABLE [dbo].[PageSlug] (
    [Id]         BIGINT         IDENTITY (1, 1) NOT NULL,
    [LanguageId] BIGINT         NOT NULL,
    [EntityId]   BIGINT         NOT NULL,
    [EntityName] VARCHAR (150)  NOT NULL,
    [Name]       NVARCHAR (400) NOT NULL,
    [Active]     BIT            NOT NULL,
    CONSTRAINT [PK_PageSlug] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_PageSlug_Language] FOREIGN KEY ([LanguageId]) REFERENCES [dbo].[Language] ([Id])
);










GO
CREATE NONCLUSTERED INDEX [NIX_PageSlug_LanguageId]
    ON [dbo].[PageSlug]([LanguageId] ASC) WITH (FILLFACTOR = 90);

