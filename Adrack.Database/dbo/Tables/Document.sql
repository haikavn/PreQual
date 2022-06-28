CREATE TABLE [dbo].[Document] (
    [Id]          BIGINT        IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (255) NULL,
    [Path]        VARCHAR (255) NULL,
    [Type]        VARCHAR (50)  NULL,
    [AffiliateId] BIGINT        NULL,
    [UserId]      BIGINT        NULL,
    [Created]     DATETIME      NULL,
    CONSTRAINT [PK_Document] PRIMARY KEY CLUSTERED ([Id] ASC)
);

