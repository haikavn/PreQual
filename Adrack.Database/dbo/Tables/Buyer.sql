CREATE TABLE [dbo].[Buyer] (
    [Id]                      BIGINT         IDENTITY (1, 1) NOT NULL,
    [CreatedOn]               DATETIME       NOT NULL,
    [Name]                    VARCHAR (50)   NULL,
    [Email]                   VARCHAR (50)   NULL,
    [Status]                  SMALLINT       NULL,
    [ManagerId]               BIGINT         NULL,
    [CountryId]               BIGINT         NULL,
    [StateProvinceId]         BIGINT         NULL,
    [AddressLine1]            VARCHAR (150)  NULL,
    [AddressLine2]            VARCHAR (150)  NULL,
    [City]                    VARCHAR (100)  NULL,
    [ZipPostalCode]           VARCHAR (20)   NULL,
    [Phone]                   VARCHAR (50)   NULL,
    [BillFrequency]           VARCHAR (10)   NULL,
    [FrequencyValue]          INT            NULL,
    [LastPostedSold]          DATETIME       NULL,
    [LastPosted]              DATETIME       NULL,
    [AlwaysSoldOption]        SMALLINT       NULL,
    [MaxDuplicateDays]        SMALLINT       NULL,
    [DailyCap]                INT            NULL,
    [Description]             VARCHAR (2000) NULL,
    [ExternalId]              BIGINT         NULL,
    [Deleted]                 BIT            NULL,
    [IsBiWeekly]              BIT            NULL,
    [CoolOffEnabled]          BIT            NULL,
    [CoolOffStart]            DATETIME       NULL,
    [CoolOffEnd]              DATETIME       NULL,
    [DoNotPresentStatus]      SMALLINT       NULL,
    [DoNotPresentUrl]         VARCHAR (300)  NULL,
    [DoNotPresentResultField] VARCHAR (50)   NULL,
    [DoNotPresentResultValue] VARCHAR (50)   NULL,
    [CanSendLeadId]           BIT            NULL,
    [DoNotPresentRequest]     VARCHAR (2000) NULL,
    [DoNotPresentPostMethod]  VARCHAR (50)   NULL,
    [AccountId]               INT            NULL,
    CONSTRAINT [PK_Buyer] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Buyer_Country] FOREIGN KEY ([CountryId]) REFERENCES [dbo].[Country] ([Id]),
    CONSTRAINT [FK_Buyer_StateProvince] FOREIGN KEY ([StateProvinceId]) REFERENCES [dbo].[StateProvince] ([Id])
);




























































GO



GO



GO
CREATE NONCLUSTERED INDEX [NIX_Buyer_StateProvinceId]
    ON [dbo].[Buyer]([StateProvinceId] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [NIX_Buyer_ManagerId]
    ON [dbo].[Buyer]([ManagerId] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [NIX_Buyer_CountryId]
    ON [dbo].[Buyer]([CountryId] ASC) WITH (FILLFACTOR = 90);

