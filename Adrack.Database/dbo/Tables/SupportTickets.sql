CREATE TABLE [dbo].[SupportTickets] (
    [Id]        BIGINT         IDENTITY (1, 1) NOT NULL,
    [UserID]    BIGINT         NOT NULL,
    [ManagerID] BIGINT         NOT NULL,
    [Subject]   VARCHAR (255)  NULL,
    [Message]   VARCHAR (1024) NULL,
    [Status]    INT            NOT NULL,
    [Priority]  INT            NOT NULL,
    [DateTime]  DATETIME       NOT NULL,
    CONSTRAINT [PK_SupportTickets] PRIMARY KEY CLUSTERED ([Id] ASC)
);



