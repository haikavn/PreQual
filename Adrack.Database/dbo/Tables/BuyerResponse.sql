CREATE TABLE [dbo].[BuyerResponse] (
    [Id]             BIGINT        IDENTITY (1, 1) NOT NULL,
    [LeadId]         BIGINT        NULL,
    [BuyerId]        BIGINT        NULL,
    [Created]        DATETIME      NULL,
    [Response]       VARCHAR (MAX) NULL,
    [BuyerChannelId] BIGINT        NULL,
    [PostedData]     VARCHAR (MAX) NULL,
    CONSTRAINT [PK_BuyerResponse] PRIMARY KEY CLUSTERED ([Id] ASC)
);



