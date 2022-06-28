CREATE TABLE [dbo].[LeadContentDublicate] (
    [Id]               BIGINT         IDENTITY (1, 1) NOT NULL,
    [LeadId]           BIGINT         NULL,
    [Ip]               VARCHAR (128)  NULL,
    [Minprice]         SMALLMONEY     NULL,
    [Firstname]        NVARCHAR (128) NULL,
    [Lastname]         NVARCHAR (128) NULL,
    [Address]          NVARCHAR (128) NULL,
    [City]             NVARCHAR (128) NULL,
    [State]            NVARCHAR (128) NULL,
    [Zip]              NVARCHAR (128) NULL,
    [HomePhone]        NVARCHAR (128) NULL,
    [CellPhone]        NVARCHAR (128) NULL,
    [Email]            VARCHAR (128)  NULL,
    [PayFrequency]     NVARCHAR (128)  NULL,
    [Directdeposit]    NVARCHAR (128)   NULL,
    [AccountType]      NVARCHAR (128)  NULL,
    [IncomeType]       NVARCHAR (128)  NULL,
    [NetMonthlyIncome] MONEY          NULL,
    [Emptime]          SMALLINT       NULL,
    [AddressMonth]     SMALLINT       NULL,
    [Dob]              DATETIME       NULL,
    [Age]              SMALLINT       NULL,
    [RequestedAmount]  MONEY          NULL,
    [Ssn]              VARCHAR (150)  NULL,
    [String1]          NVARCHAR(128)  NULL,
    [String2]          NVARCHAR(128)  NULL,
    [String3]          NVARCHAR(128)  NULL,
    [String4]          NVARCHAR(128)  NULL,
    [String5]          NVARCHAR(128)  NULL,
    [Created]          DATETIME       NULL,
    [AffiliateId]      BIGINT         NULL,
    [CampaignType]     SMALLINT       NULL,
    [OriginalLeadId]   BIGINT         NULL,
    [CreateArchiveDT]  DATETIME       CONSTRAINT [DF_LeadContentDublicate_CreateArchiveDT] DEFAULT (getutcdate()) NOT NULL,
    [BankPhone]        VARCHAR (128)  NULL,
    [AffiliateName] VARCHAR(150) NULL, 
    CONSTRAINT [PK_LeadContentDublicate] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_LeadContentDublicate_Affiliate] FOREIGN KEY ([AffiliateId]) REFERENCES [dbo].[Affiliate] ([Id]),
    CONSTRAINT [FK_LeadContentDublicate_LeadMain] FOREIGN KEY ([LeadId]) REFERENCES [dbo].[LeadMain] ([Id])
);




















GO
CREATE NONCLUSTERED INDEX [NIX_LeadContentDublicate_LeadId]
    ON [dbo].[LeadContentDublicate]([LeadId] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [NIX_LeadContentDublicate_AffiliateId]
    ON [dbo].[LeadContentDublicate]([AffiliateId] ASC);


GO
CREATE NONCLUSTERED INDEX [NIX_LeadContentDublicate_SSN]
    ON [dbo].[LeadContentDublicate]([Ssn] ASC);

