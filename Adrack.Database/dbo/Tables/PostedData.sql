CREATE TABLE [dbo].[PostedData] (
    [Id]             BIGINT        IDENTITY (1, 1) NOT NULL,
    [LeadId]         BIGINT        NULL,
    [BuyerChannelId] BIGINT        NULL,
    [Posted]         VARCHAR (MAX) NULL,
    [Created]        DATETIME      NULL,
    [MinPrice]       MONEY         NULL,
    [Status]         SMALLINT      NULL,
    CONSTRAINT [PK_PostedData] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_PostedData_BuyerChannel] FOREIGN KEY ([BuyerChannelId]) REFERENCES [dbo].[BuyerChannel] ([Id]),
    CONSTRAINT [FK_PostedData_LeadMain] FOREIGN KEY ([LeadId]) REFERENCES [dbo].[LeadMain] ([Id])
);








GO
CREATE NONCLUSTERED INDEX [nci_wi_PostedData_969CB9DF1B5DFA34FB4CE0519D0277F4]
    ON [dbo].[PostedData]([LeadId] ASC, [BuyerChannelId] ASC, [MinPrice] ASC)
    INCLUDE([Created], [Posted]);

