CREATE PROCEDURE [dbo].[CheckForDublicate]
	-- Add the parameters for the stored procedure here
	@ssn varchar(150),
	@leadId bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT t1.* from (select TOP(1) [Id]
		  ,[LeadId]
		  ,[Ip]
		  ,[Minprice]
		  ,[Firstname]
		  ,[Lastname]
		  ,[Address]
		  ,[City]
		  ,[State]
		  ,[Zip]
		  ,[HomePhone]
		  ,[CellPhone]
		  ,[BankPhone]
		  ,[Email]
		  ,[PayFrequency]
		  ,[Directdeposit]
		  ,[AccountType]
		  ,[IncomeType]
		  ,[NetMonthlyIncome]
		  ,[Emptime]
		  ,[AddressMonth]
		  ,[Dob]
		  ,[Age]
		  ,[RequestedAmount]
		  ,[Ssn]
		  ,[String1]
		  ,[String2]
		  ,[String3]
		  ,[String4]
		  ,[String5]
		  ,[Created]
		  ,[AffiliateId]
		  ,[CampaignType]
		  ,[MinpriceStr]
		  ,[AffiliateSubId]
		  ,[AffiliateSubId2]
	  FROM [dbo].[LeadContent]
	  where Created >= DATEADD(month, -6, getdate()) and LeadId != @leadId and ssn = @ssn and campaigntype = 0 order by id desc) as t1
	  union 
		SELECT t1.* from (select TOP(1) [Id]
			  ,[LeadId]
			  ,[Ip]
			  ,[Minprice]
			  ,[Firstname]
			  ,[Lastname]
			  ,[Address]
			  ,[City]
			  ,[State]
			  ,[Zip]
			  ,[HomePhone]
			  ,[CellPhone]
			  ,[BankPhone]
			  ,[Email]
			  ,[PayFrequency]
			  ,[Directdeposit]
			  ,[AccountType]
			  ,[IncomeType]
			  ,[NetMonthlyIncome]
			  ,[Emptime]
			  ,[AddressMonth]
			  ,[Dob]
			  ,[Age]
			  ,[RequestedAmount]
			  ,[Ssn]
			  ,[String1]
			  ,[String2]
			  ,[String3]
			  ,[String4]
			  ,[String5]
			  ,[Created]
			  ,[AffiliateId]
			  ,[CampaignType]
			  ,[MinpriceStr]
			  ,[AffiliateSubId]
			  ,[AffiliateSubId2]
		  FROM [dbo].[LeadContent]
		  where Created >= DATEADD(month, -6, getdate()) and LeadId != @leadId and ssn = @ssn and campaigntype = 0 order by id) as t1
END