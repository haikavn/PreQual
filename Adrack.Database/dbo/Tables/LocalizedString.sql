CREATE TABLE [dbo].[LocalizedString] (
    [Id]         BIGINT          IDENTITY (1, 1) NOT NULL,
    [LanguageId] BIGINT          NOT NULL,
    [Key]        VARCHAR (250)   NOT NULL,
    [Value]      NVARCHAR (1000) NOT NULL,
    CONSTRAINT [PK_LocalizedString] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_LocalizedString_Language] FOREIGN KEY ([LanguageId]) REFERENCES [dbo].[Language] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [NIX_LocalizedString_LanguageId]
    ON [dbo].[LocalizedString]([LanguageId] ASC) WITH (FILLFACTOR = 90);

