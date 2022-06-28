CREATE TABLE [dbo].[LocalizedProperty] (
    [Id]         BIGINT          IDENTITY (1, 1) NOT NULL,
    [EntityId]   BIGINT          NOT NULL,
    [LanguageId] BIGINT          NOT NULL,
    [KeyGroup]   VARCHAR (250)   NOT NULL,
    [Key]        VARCHAR (250)   NOT NULL,
    [Value]      NVARCHAR (1000) NOT NULL,
    CONSTRAINT [PK_LocalizedProperty] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_LocalizedProperty_Language] FOREIGN KEY ([LanguageId]) REFERENCES [dbo].[Language] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [NIX_LocalizedProperty_LanguageId]
    ON [dbo].[LocalizedProperty]([LanguageId] ASC) WITH (FILLFACTOR = 90);

