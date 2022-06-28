-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[CloneBuyerChannel] 
	-- Add the parameters for the stored procedure here
	@buyerChannelId bigint,
	@name varchar(200)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [dbo].[BuyerChannel]
			   ([CampaignId]
			   ,[XmlTemplate]
			   ,[BuyerId]
			   ,[Name]
			   ,[Status]
			   ,[PostingUrl]
			   ,[AcceptedField]
			   ,[AcceptedValue]
			   ,[AcceptedFrom]
			   ,[ErrorField]
			   ,[ErrorValue]
			   ,[ErrorFrom]
			   ,[RejectedField]
			   ,[RejectedValue]
			   ,[RejectedFrom]
			   ,[TestField]
			   ,[TestValue]
			   ,[TestFrom]
			   ,[MessageField]
			   ,[RedirectField]
			   ,[PriceField]
			   ,[DeliveryMethod]
			   ,[Timeout]
			   ,[AfterTimeout]
			   ,[NotificationEmail]
			   ,[AffiliatePrice]
			   ,[BuyerPrice]
			   ,[CapReachedNotification]
			   ,[TimeoutNotification]
			   ,[OrderNum]
			   ,[IsFixed]
			   ,[AllowedAffiliateChannels]
			   ,[DataFormat]
			   ,[PostingHeaders]
			   ,[BuyerPriceOption]
			   ,[AffiliatePriceOption]
			   ,[AlwaysSoldOption]
			   ,[RedirectUrl]
			   ,[ZipCodeTargeting]
			   ,[StateTargeting]
			   ,[MinAgeTargeting]
			   ,[MaxAgeTargeting]
			   ,[EnableZipCodeTargeting]
			   ,[EnableStateTargeting]
			   ,[EnableAgeTargeting]
			   ,[ZipCodeCondition]
			   ,[StateCondition])
	SELECT [CampaignId]
		  ,[XmlTemplate]
		  ,[BuyerId]
		  ,@name
		  ,[Status]
		  ,[PostingUrl]
		  ,[AcceptedField]
		  ,[AcceptedValue]
		  ,[AcceptedFrom]
		  ,[ErrorField]
		  ,[ErrorValue]
		  ,[ErrorFrom]
		  ,[RejectedField]
		  ,[RejectedValue]
		  ,[RejectedFrom]
		  ,[TestField]
		  ,[TestValue]
		  ,[TestFrom]
		  ,[MessageField]
		  ,[RedirectField]
		  ,[PriceField]
		  ,[DeliveryMethod]
		  ,[Timeout]
		  ,[AfterTimeout]
		  ,[NotificationEmail]
		  ,[AffiliatePrice]
		  ,[BuyerPrice]
		  ,[CapReachedNotification]
		  ,[TimeoutNotification]
		  ,[OrderNum]
		  ,[IsFixed]
		  ,[AllowedAffiliateChannels]
		  ,[DataFormat]
		  ,[PostingHeaders]
		  ,[BuyerPriceOption]
		  ,[AffiliatePriceOption]
		  ,[AlwaysSoldOption]
		  ,[RedirectUrl]
		  ,[ZipCodeTargeting]
		  ,[StateTargeting]
		  ,[MinAgeTargeting]
		  ,[MaxAgeTargeting]
		  ,[EnableZipCodeTargeting]
		  ,[EnableStateTargeting]
		  ,[EnableAgeTargeting]
		  ,[ZipCodeCondition]
		  ,[StateCondition]
	  FROM [dbo].[BuyerChannel] where Id = @buyerChannelId;

	  declare @Id bigint;

	  select @Id = scope_identity();

	INSERT INTO [dbo].[BuyerChannelTemplate]
			   ([CampaignTemplateId]
			   ,[TemplateField]
			   ,[SectionName]
			   ,[BuyerChannelId]
			   ,[DefaultValue])
	  SELECT [CampaignTemplateId]
		  ,[TemplateField]
		  ,[SectionName]
		  ,@Id
		  ,[DefaultValue]
	  FROM [dbo].[BuyerChannelTemplate] where buyerchannelid = @buyerChannelId;


	INSERT INTO [dbo].[BuyerChannelFilterCondition]
			   ([Value]
			   ,[Condition]
			   ,[ConditionOperator]
			   ,[CampaignTemplateId]
			   ,[BuyerChannelId]
			   ,[Value2])
	  SELECT [Value]
		  ,[Condition]
		  ,[ConditionOperator]
		  ,[CampaignTemplateId]	
		  ,@Id	  
		  ,[Value2]
	  FROM [dbo].[BuyerChannelFilterCondition] where buyerchannelid = @buyerChannelId;

	  select @Id;
END