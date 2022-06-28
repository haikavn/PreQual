CREATE TABLE [dbo].[BuyerChannel] (
    [Id]                       BIGINT         IDENTITY (1, 1) NOT NULL,
    [CampaignId]               BIGINT         NULL,
    [XmlTemplate]              VARCHAR (MAX)  NULL,
    [BuyerId]                  BIGINT         NULL,
    [Name]                     VARCHAR (50)   NULL,
    [Status]                   SMALLINT       NULL,
    [PostingUrl]               VARCHAR (1000) NULL,
    [AcceptedField]            VARCHAR (50)   NULL,
    [AcceptedValue]            VARCHAR (50)   NULL,
    [AcceptedFrom]             SMALLINT       NULL,
    [ErrorField]               VARCHAR (50)   NULL,
    [ErrorValue]               VARCHAR (50)   NULL,
    [ErrorFrom]                SMALLINT       NULL,
    [RejectedField]            VARCHAR (50)   NULL,
    [RejectedValue]            VARCHAR (50)   NULL,
    [RejectedFrom]             SMALLINT       NULL,
    [TestField]                VARCHAR (50)   NULL,
    [TestValue]                VARCHAR (50)   NULL,
    [TestFrom]                 SMALLINT       NULL,
    [MessageField]             VARCHAR (50)   NULL,
    [RedirectField]            VARCHAR (50)   NULL,
    [PriceField]               VARCHAR (50)   NULL,
    [DeliveryMethod]           SMALLINT       NULL,
    [Timeout]                  SMALLINT       NULL,
    [AfterTimeout]             SMALLINT       NULL,
    [NotificationEmail]        VARCHAR (1000) NULL,
    [AffiliatePrice]           SMALLMONEY     NULL,
    [BuyerPrice]               SMALLMONEY     NULL,
    [CapReachedNotification]   BIT            NULL,
    [TimeoutNotification]      BIT            NULL,
    [OrderNum]                 INT            NULL,
    [IsFixed]                  BIT            NULL,
    [AllowedAffiliateChannels] VARCHAR (MAX)  NULL,
    [DataFormat]               SMALLINT       NULL,
    [PostingHeaders]           VARCHAR (MAX)  NULL,
    [BuyerPriceOption]         SMALLINT       NULL,
    [AffiliatePriceOption]     SMALLINT       NULL,
    [AlwaysSoldOption]         SMALLINT       NULL,
    [RedirectUrl]              VARCHAR (300)  NULL,
    [ZipCodeTargeting]         VARCHAR (MAX)  NULL,
    [StateTargeting]           VARCHAR (MAX)  NULL,
    [MinAgeTargeting]          SMALLINT       NULL,
    [MaxAgeTargeting]          SMALLINT       NULL,
    [EnableZipCodeTargeting]   BIT            NULL,
    [EnableStateTargeting]     BIT            NULL,
    [EnableAgeTargeting]       BIT            NULL,
    [ZipCodeCondition]         SMALLINT       NULL,
    [StateCondition]           SMALLINT       NULL,
    [Deleted]                  BIT            NULL,
    [Holidays]                 VARCHAR (2000) NULL,
    [MaxDuplicateDays]         SMALLINT       NULL,
    [TimeZone]                 FLOAT (53)     NULL,
    [TimeZoneStr]              VARCHAR (50)   NULL,
    [LeadAcceptRate]           FLOAT (53)     NULL,
    [SubIdWhiteListEnabled]    BIT            NULL,
    [AccountIdField]           VARCHAR (50)   NULL,
    [EnableCustomPriceReject]  BIT            NULL,
    [PriceRejectWinResponse]   VARCHAR (2000) NULL,
    [FieldAppendEnabled]       BIT            NULL,
    [WinResponseUrl]           VARCHAR (500)  NULL,
    [WinResponsePostMethod]    VARCHAR (50)   NULL,
    [LeadIdField]              VARCHAR (50)   NULL,
    [ChildChannels]            VARCHAR (MAX)  NULL,
    [PriceRejectField]         VARCHAR (50)   NULL,
    [Delimeter]                VARCHAR (50)   NULL,
    [PriceRejectValue]         VARCHAR (50)   NULL,
    [ChannelMappingUniqueId]   VARCHAR (50)   NULL,
    [ResponseFormat]           SMALLINT       NULL,
    [StatusStr]                VARCHAR (100)  NULL,
    [StatusExpireDate]         DATETIME       NULL,
    [StatusAutoChange]         BIT            NULL,
    [StatusChangeMinutes]      SMALLINT       NULL,
    [DailyCap]                 INT            NULL,
    [Note]                     VARCHAR (MAX)  NULL,
    [CapReachEmailCount]       SMALLINT       NULL,
    [ChangeStatusAfterCount]   SMALLINT       NULL,
    [CurrentStatusChangeNum]   SMALLINT       NULL,
    CONSTRAINT [PK_BuyerChannel] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_BuyerChannel_Buyer] FOREIGN KEY ([BuyerId]) REFERENCES [dbo].[Buyer] ([Id]),
    CONSTRAINT [FK_BuyerChannel_Campaign] FOREIGN KEY ([CampaignId]) REFERENCES [dbo].[Campaign] ([Id])
);


































































GO
CREATE NONCLUSTERED INDEX [NIX_BuyerChannel_CampaignId]
    ON [dbo].[BuyerChannel]([CampaignId] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [NIX_BuyerChannel_BuyerId]
    ON [dbo].[BuyerChannel]([BuyerId] ASC) WITH (FILLFACTOR = 90);

