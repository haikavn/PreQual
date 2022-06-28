CREATE TABLE [dbo].[LeadNote] (
    [Id]          BIGINT        IDENTITY (1, 1) NOT NULL,
    [LeadId]      BIGINT        NOT NULL,
    [NoteTitleId] SMALLINT      NOT NULL,
    [Note]        VARCHAR (255) NULL,
    [Created]     DATETIME      NULL,
    [Author]      VARCHAR (50)  NULL,
    CONSTRAINT [PK_LeadNote] PRIMARY KEY CLUSTERED ([Id] ASC)
);





