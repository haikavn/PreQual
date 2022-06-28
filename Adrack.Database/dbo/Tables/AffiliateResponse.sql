CREATE TABLE [dbo].[AffiliateResponse] (
    [Id]                 BIGINT         IDENTITY (1, 1) NOT NULL,
    [LeadId]             BIGINT         NULL,
    [AffiliateId]        BIGINT         NULL,
    [Created]            DATETIME       NULL,
    [Response]           VARCHAR (MAX)  NULL,
    [AffiliateChannelId] BIGINT         NULL,
    [MinPrice]           MONEY          NULL,
    [ProcessStartedAt]   DATETIME       NULL,
    [Message]            VARCHAR (1000) NULL,
    [Status]             SMALLINT       NULL,
    [ErrorType]          SMALLINT       NULL,
    [Validator]          SMALLINT       NULL,
    [ReceivedData]       VARCHAR (MAX)  NULL,
    [State]              VARCHAR (50)   NULL,
    CONSTRAINT [PK_AffiliateResponse] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AffiliateResponse_Affiliate] FOREIGN KEY ([AffiliateId]) REFERENCES [dbo].[Affiliate] ([Id]),
    CONSTRAINT [FK_AffiliateResponse_AffiliateChannel] FOREIGN KEY ([AffiliateChannelId]) REFERENCES [dbo].[AffiliateChannel] ([Id]),
    CONSTRAINT [FK_AffiliateResponse_LeadMain] FOREIGN KEY ([LeadId]) REFERENCES [dbo].[LeadMain] ([Id])
);














GO
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20180704-130705]
    ON [dbo].[AffiliateResponse]([Created] ASC, [ErrorType] ASC, [Validator] ASC);


GO
CREATE NONCLUSTERED INDEX [NIX_LeadId]
    ON [dbo].[AffiliateResponse]([LeadId] ASC);

