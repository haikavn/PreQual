CREATE TABLE [dbo].[BuyerChannelSchedule] (
    [Id]             BIGINT   IDENTITY (1, 1) NOT NULL,
    [DayValue]       SMALLINT NULL,
    [FromTime]       INT      NULL,
    [ToTime]         INT      NULL,
    [Quantity]       INT      NULL,
    [BuyerChannelId] BIGINT   NULL,
    [PostedWait]     INT      NULL,
    [SoldWait]       INT      NULL,
    [HourMax]        INT      NULL,
    [Price]          MONEY    NULL,
    [LeadStatus]     SMALLINT NULL,
    CONSTRAINT [PK_BuyerChannelSchedule] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_BuyerChannelSchedule_BuyerChannel] FOREIGN KEY ([BuyerChannelId]) REFERENCES [dbo].[BuyerChannel] ([Id])
);














GO
CREATE NONCLUSTERED INDEX [NIX_BuyerChannelSchedule_BuyerChannelId]
    ON [dbo].[BuyerChannelSchedule]([BuyerChannelId] ASC) WITH (FILLFACTOR = 90);

