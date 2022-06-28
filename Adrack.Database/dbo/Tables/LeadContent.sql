CREATE TABLE [dbo].[LeadContent] (
    [Id]               BIGINT        IDENTITY (1, 1) NOT NULL,
    [LeadId]           BIGINT        NULL,
    [Ip]               VARCHAR (128) NULL,
    [Minprice]         SMALLMONEY    NULL,
    [Firstname]        VARCHAR (128) NULL,
    [Lastname]         VARCHAR (128) NULL,
    [Address]          VARCHAR (128) NULL,
    [City]             VARCHAR (128) NULL,
    [State]            VARCHAR (128) NULL,
    [Zip]              VARCHAR (128) NULL,
    [HomePhone]        VARCHAR (128) NULL,
    [CellPhone]        VARCHAR (128) NULL,
    [Email]            VARCHAR (128) NULL,
    [PayFrequency]     VARCHAR (128) NULL,
    [Directdeposit]    VARCHAR (128) NULL,
    [AccountType]      VARCHAR (128) NULL,
    [IncomeType]       VARCHAR (128) NULL,
    [NetMonthlyIncome] MONEY         NULL,
    [Emptime]          SMALLINT      NULL,
    [AddressMonth]     SMALLINT      NULL,
    [Dob]              DATETIME      NULL,
    [Age]              SMALLINT      NULL,
    [RequestedAmount]  MONEY         NULL,
    [Ssn]              VARCHAR (150) NULL,
    [String1]          VARCHAR (128) NULL,
    [String2]          VARCHAR (128) NULL,
    [String3]          VARCHAR (128) NULL,
    [String4]          VARCHAR (128) NULL,
    [String5]          VARCHAR (128) NULL,
    [Created]          DATETIME      NULL,
    [AffiliateId]      BIGINT        NULL,
    [CampaignType]     SMALLINT      NULL,
    [MinpriceStr]      VARCHAR (128) NULL,
    [AffiliateSubId]   VARCHAR (128) NULL,
    [AffiliateSubId2]  VARCHAR (128) NULL,
    [CreateArchiveDT]  DATETIME      CONSTRAINT [DF_LeadContent_CreateArchiveDT] DEFAULT (getutcdate()) NOT NULL,
    [BankPhone]        VARCHAR (128) NULL,
    CONSTRAINT [PK_LeadContent] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_LeadContent_Affiliate] FOREIGN KEY ([AffiliateId]) REFERENCES [dbo].[Affiliate] ([Id]),
    CONSTRAINT [FK_LeadContent_LeadMain] FOREIGN KEY ([LeadId]) REFERENCES [dbo].[LeadMain] ([Id])
);










































GO
CREATE NONCLUSTERED INDEX [NIX_LeadContent_LeadId]
    ON [dbo].[LeadContent]([LeadId] ASC) WITH (FILLFACTOR = 90);


GO



GO
CREATE NONCLUSTERED INDEX [NIX_LeadContent_Include]
    ON [dbo].[LeadContent]([LeadId] ASC)
    INCLUDE([AccountType], [Address], [AddressMonth], [AffiliateId], [Age], [CampaignType], [CellPhone], [City], [Created], [Directdeposit], [Dob], [Email], [Emptime], [Firstname], [HomePhone], [Id], [IncomeType], [Ip], [Lastname], [Minprice], [NetMonthlyIncome], [PayFrequency], [RequestedAmount], [Ssn], [State], [String1], [String2], [String3], [String4], [String5], [Zip]) WITH (FILLFACTOR = 90);


GO

CREATE TRIGGER [dbo].[SetTreeMinuteLead]
   ON  [dbo].[LeadContent]
   AFTER INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	return;
	declare @maxid bigint;
	select @maxid = max(Id) from [LeadMain];

	UPDATE R 
	SET R.LeadNumber = ISNULL((select top 1 LeadId from dbo.[LeadContent] where LeadId>@maxid-10000 and Ssn = p.Ssn and AffiliateId = p.AffiliateId and Created >= dateadd(minute, -3, p.Created)), p.LeadId)
	FROM dbo.LeadMain AS R
	INNER JOIN inserted AS P 
		   ON R.Id = P.LeadId 
END
GO
CREATE NONCLUSTERED INDEX [NIX_Ssn_CampaignType_LeadId_Created_INCLUDE]
    ON [dbo].[LeadContent]([Ssn] ASC, [CampaignType] ASC, [LeadId] ASC, [Created] ASC)
    INCLUDE([Ip], [Minprice], [Firstname], [Lastname], [Address], [City], [State], [Zip], [HomePhone], [CellPhone], [Email], [PayFrequency], [Directdeposit], [AccountType], [IncomeType], [NetMonthlyIncome], [Emptime], [AddressMonth], [Dob], [Age], [RequestedAmount], [String1], [String2], [String3], [String4], [String5], [AffiliateId], [MinpriceStr], [AffiliateSubId], [AffiliateSubId2]) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [NIX_Ssn_Create_AffilateId]
    ON [dbo].[LeadContent]([Ssn] ASC, [Created] ASC, [AffiliateId] ASC);

