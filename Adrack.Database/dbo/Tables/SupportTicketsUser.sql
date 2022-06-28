CREATE TABLE [dbo].[SupportTicketsUser] (
    [Id]       BIGINT IDENTITY (1, 1) NOT NULL,
    [TicketID] BIGINT NOT NULL,
    [UserID]   BIGINT NOT NULL,
    CONSTRAINT [PK_SupportTicketsUser] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SupportTicketsUser_SupportTickets] FOREIGN KEY ([TicketID]) REFERENCES [dbo].[SupportTickets] ([Id]),
    CONSTRAINT [FK_SupportTicketsUser_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User] ([Id])
);




GO
CREATE NONCLUSTERED INDEX [NIX_SupportTicketsUser_UserID]
    ON [dbo].[SupportTicketsUser]([UserID] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [NIX_SupportTicketsUser_TicketID]
    ON [dbo].[SupportTicketsUser]([TicketID] ASC) WITH (FILLFACTOR = 90);

