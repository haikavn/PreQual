CREATE TABLE [dbo].[AffiliateNote] (
    [Id]          BIGINT         IDENTITY (1, 1) NOT NULL,
    [Created]     DATETIME       NULL,
    [Note]        VARCHAR (1000) NULL,
    [AffiliateId] BIGINT         NULL,
    CONSTRAINT [PK_AffiliateNotes] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AffiliateNotes_Affiliate] FOREIGN KEY ([AffiliateId]) REFERENCES [dbo].[Affiliate] ([Id])
);

