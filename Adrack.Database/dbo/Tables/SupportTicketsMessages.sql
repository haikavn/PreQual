CREATE TABLE [dbo].[SupportTicketsMessages] (
    [Id]       BIGINT        IDENTITY (1, 1) NOT NULL,
    [TicketID] BIGINT        NOT NULL,
    [Message]  TEXT          NULL,
    [FilePath] VARCHAR (255) NULL,
    [DateTime] DATETIME      NULL,
    [AuthorID] BIGINT        NULL,
    [IsNew]    BIT           CONSTRAINT [DF_SupportTicketsMessages_IsNew] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_SupportTicketsMessages] PRIMARY KEY CLUSTERED ([Id] ASC)
);



