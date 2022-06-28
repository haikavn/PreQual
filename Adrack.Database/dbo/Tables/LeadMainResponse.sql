CREATE TABLE [dbo].[LeadMainResponse] (
    [Id]                 BIGINT         IDENTITY (1, 1) NOT NULL,
    [BuyerId]            BIGINT         NULL,
    [BuyerChannelId]     BIGINT         NULL,
    [Response]           VARCHAR (MAX) NULL,
    [ResponseTime]       FLOAT (53)     NULL,
    [LeadId]             BIGINT         NULL,
    [AffiliateChannelId] BIGINT         NULL,
    [AffiliateId]        BIGINT         NULL,
    [CampaignId]         BIGINT         NULL,
    [ResponseError]      VARCHAR (2000)  NULL,
    [Status]             SMALLINT       NULL,
    [AffiliatePrice]     SMALLMONEY     NULL,
    [BuyerPrice]         SMALLMONEY     NULL,
    [CampaignType]       SMALLINT       NULL,
    [State]              VARCHAR (50)   NULL,
    [Created]            DATETIME       NULL,
    [MinPrice]           MONEY          NULL,
    [ArchiveDateTime]    DATETIME       CONSTRAINT [DF_LeadMainResponse_ArchiveDateTime] DEFAULT (getutcdate()) NOT NULL,
    [ErrorType]          SMALLINT       NULL,
    [Validator]          SMALLINT       NULL,
    [Ssn]                VARCHAR (150)  NULL,
    CONSTRAINT [PK_LeadMainResponse] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_LeadMainResponse_Affiliate] FOREIGN KEY ([AffiliateChannelId]) REFERENCES [dbo].[AffiliateChannel] ([Id]),
    CONSTRAINT [FK_LeadMainResponse_Affiliate1] FOREIGN KEY ([AffiliateId]) REFERENCES [dbo].[Affiliate] ([Id]),
    CONSTRAINT [FK_LeadMainResponse_Buyer] FOREIGN KEY ([BuyerId]) REFERENCES [dbo].[Buyer] ([Id]),
    CONSTRAINT [FK_LeadMainResponse_BuyerChannel] FOREIGN KEY ([BuyerChannelId]) REFERENCES [dbo].[BuyerChannel] ([Id]),
    CONSTRAINT [FK_LeadMainResponse_Campaign] FOREIGN KEY ([CampaignId]) REFERENCES [dbo].[Campaign] ([Id]),
    CONSTRAINT [FK_LeadMainResponse_LeadMain] FOREIGN KEY ([LeadId]) REFERENCES [dbo].[LeadMain] ([Id])
);




























GO
CREATE NONCLUSTERED INDEX [NIX_LeadMainResponse_LeadId]
    ON [dbo].[LeadMainResponse]([LeadId] ASC) WITH (FILLFACTOR = 90);


GO



GO



GO
CREATE NONCLUSTERED INDEX [NIX_LeadMainResponse_BuyerChannelId]
    ON [dbo].[LeadMainResponse]([BuyerChannelId] ASC) WITH (FILLFACTOR = 90);


GO



GO



GO
CREATE NONCLUSTERED INDEX [NIX_LeadMainResponse_Include]
    ON [dbo].[LeadMainResponse]([Status] ASC, [LeadId] ASC)
    INCLUDE([BuyerChannelId], [BuyerId]) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [NIX_LeadMainResponse_BuyerChannelId_Status_Created]
    ON [dbo].[LeadMainResponse]([BuyerChannelId] ASC, [Status] ASC, [Created] ASC)
    INCLUDE([LeadId]);


GO
CREATE NONCLUSTERED INDEX [NIX_Status_Include_LeadId_AddPrice_BuyerPrice]
    ON [dbo].[LeadMainResponse]([Status] ASC)
    INCLUDE([LeadId], [AffiliatePrice], [BuyerPrice]);


GO
CREATE NONCLUSTERED INDEX [NIX_LeadMainResponse_Created_Error_Validator]
    ON [dbo].[LeadMainResponse]([Created] ASC, [ErrorType] ASC, [Validator] ASC);


GO
CREATE NONCLUSTERED INDEX [NIX_Ssn_Create_BuyerId]
    ON [dbo].[LeadMainResponse]([Ssn] ASC, [Created] ASC, [BuyerId] ASC);


GO
CREATE NONCLUSTERED INDEX [NIX_Ssn_Create_BuyerChannelId]
    ON [dbo].[LeadMainResponse]([Ssn] ASC, [Created] ASC, [BuyerChannelId] ASC);

