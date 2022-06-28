CREATE TABLE [dbo].[LeadTransaction] (
    [Id]        BIGINT   IDENTITY (1, 1) NOT NULL,
    [CreatedOn] DATETIME NOT NULL,
    CONSTRAINT [PK_LeadTransaction] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

