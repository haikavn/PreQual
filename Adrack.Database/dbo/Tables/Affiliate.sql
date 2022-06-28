CREATE TABLE [dbo].[Affiliate] (
    [Id]                          BIGINT         IDENTITY (1, 1) NOT NULL,
    [CountryId]                   BIGINT         NULL,
    [StateProvinceId]             BIGINT         NULL,
    [Name]                        VARCHAR (50)   NULL,
    [AddressLine1]                VARCHAR (150)  NULL,
    [AddressLine2]                VARCHAR (150)  NULL,
    [City]                        VARCHAR (100)  NULL,
    [ZipPostalCode]               VARCHAR (20)   NULL,
    [Email]                       VARCHAR (100)  NULL,
    [CreatedOn]                   DATETIME       NOT NULL,
    [Telephone]                   BIGINT         NULL,
    [UserId]                      BIGINT         NULL,
    [ManagerId]                   BIGINT         NULL,
    [Status]                      SMALLINT       NULL,
    [Phone]                       VARCHAR (50)   NULL,
    [BillFrequency]               VARCHAR (10)   NULL,
    [FrequencyValue]              INT            NULL,
    [BillWithin]                  INT            NULL,
    [RegistrationIp]              VARCHAR (20)   NULL,
    [Website]                     VARCHAR (255)  NULL,
    [CreateArchiveDT]             DATETIME       CONSTRAINT [DF_Affiliate_CreateArchiveDT] DEFAULT (getutcdate()) NOT NULL,
    [Deleted]                     BIT            NULL,
    [IsBiWeekly]                  BIT            NULL,
    [WhiteIp]                     VARCHAR (1000) NULL,
    [DefaultAffiliatePriceMethod] SMALLINT       NULL,
    [DefaultAffiliatePrice]       SMALLMONEY     NULL,
    CONSTRAINT [PK_Affiliate] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Affiliate_Country] FOREIGN KEY ([CountryId]) REFERENCES [dbo].[Country] ([Id]),
    CONSTRAINT [FK_Affiliate_StateProvince] FOREIGN KEY ([StateProvinceId]) REFERENCES [dbo].[StateProvince] ([Id])
);










































GO



GO



GO
CREATE NONCLUSTERED INDEX [NIX_Affiliate_StateProvinceId]
    ON [dbo].[Affiliate]([StateProvinceId] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [NIX_Affiliate_CountryId]
    ON [dbo].[Affiliate]([CountryId] ASC) WITH (FILLFACTOR = 90);

